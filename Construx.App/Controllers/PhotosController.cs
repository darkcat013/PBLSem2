using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Construx.App.Data;
using Construx.App.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Construx.App.Constants;
using Construx.App.Dtos.Company;
using Mapster;
using MediatR;
using Construx.App.Interfaces;
using Microsoft.AspNetCore.Identity;
using Construx.App.Domain.Identity;
using Microsoft.AspNetCore.Hosting;
using System.IO;

using Microsoft.AspNetCore.Http;

namespace Construx.App.Controllers
{
    public class PhotosController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IPhotoRepository _photoRepository;
        private readonly UserManager<User> _userManager;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public PhotosController(ApplicationDbContext context, IPhotoRepository photoRepository, UserManager<User> userManager, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _photoRepository = photoRepository;
            _userManager = userManager;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Photos
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Photos.Include(p => p.ObjectType).Include(p => p.User);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Photos/Details/5
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var photo = await _context.Photos
                .Include(p => p.ObjectType)
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (photo == null || (photo.User.UserName != User.Identity.Name && !User.IsInRole(UserRoles.Admin)))
            {
                return NotFound();
            }

            return View(photo);
        }

        // GET: Photos/Create
        public IActionResult Create()
        {
            ViewData["ObjectTypeId"] = new SelectList(_context.ObjectTypes, "Id", "Id");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // GET: Photos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var photo = await _context.Photos
                .Include(p => p.ObjectType)
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (photo == null || (photo.User.UserName != User.Identity.Name && !User.IsInRole(UserRoles.Admin)))
            {
                return NotFound();
            }

            return View(photo);
        }

        // POST: Photos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await DeletePhoto(id);
            return LocalRedirect("~/Plans/");
        }

        private bool PhotoExists(int id)
        {
            return _context.Photos.Any(e => e.Id == id);
        }

        public async Task DeletePhoto(int id)
        {
            var photo = await _context.Photos.FindAsync(id);

            string wwwRootPath = _webHostEnvironment.WebRootPath;
            var path = wwwRootPath + photo.Name;

            if (System.IO.File.Exists(path))
                System.IO.File.Delete(path);
            _context.Photos.Remove(photo);
            await _context.SaveChangesAsync();
        }
    }
}