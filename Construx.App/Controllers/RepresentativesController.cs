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
using Construx.App.Repositories;
using Construx.App.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Construx.App.Interfaces;
using Mapster;
using Construx.App.Dtos.Representative;

namespace Construx.App.Controllers
{
    public class RepresentativesController : BaseController
    {
        private readonly ApplicationDbContext _context;
        private readonly IRepresentativeRepository _representativeRepository;
        private readonly UserManager<User> _userManager;

        public RepresentativesController(ApplicationDbContext context, IRepresentativeRepository representativeRepository, UserManager<User> userManager)
        {
            _context = context;
            _representativeRepository = representativeRepository;
            _userManager = userManager;
        }

        [Authorize(Roles = UserRoles.Admin)]
        // GET: Representatives
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Representatives.Include(r => r.Company).Include(r => r.User);
            return View(await applicationDbContext.ToListAsync());
        }

        [Authorize(Roles = UserRoles.Admin)]
        // GET: Representatives/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var representative = await _context.Representatives
                .Include(r => r.Company)
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (representative == null)
            {
                return NotFound();
            }

            return View(representative);
        }

        // GET: Representatives/Create
        public async Task<IActionResult> Create()
        {
            var currUser = await _userManager.FindByNameAsync(User.Identity.Name);
            var representative = new Representative
            {
                UserId = currUser.Id
            };
            _representativeRepository.Add(representative);
            await _representativeRepository.SaveChangesAsync();
            representative = currUser.Representative;
            return View();
        }

        // POST: Representatives/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IDNP,JobTitle,CompanyExists")] CreateRepresentativeDto createRepresentativeDto)
        {
            var currUser = await _userManager.FindByNameAsync(User.Identity.Name);
            var representative = await _representativeRepository.GetByUserId(currUser.Id);
            
            if (ModelState.IsValid)
            {
                representative.IDNP = createRepresentativeDto.IDNP;
                representative.JobTitle = createRepresentativeDto.JobTitle;
                await _representativeRepository.SaveChangesAsync();

                if (!createRepresentativeDto.CompanyExists)
                {
                    return LocalRedirect("~/Companies/Create");
                }
                else
                {
                    return LocalRedirect("~/");
                }
            }
            return View(createRepresentativeDto);
        }

        [Authorize(Roles = UserRoles.Admin)]
        // GET: Representatives/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var representative = await _context.Representatives.FindAsync(id);
            if (representative == null)
            {
                return NotFound();
            }
            ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Name", representative.CompanyId);
            return View(representative);
        }

        // POST: Representatives/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = UserRoles.Admin)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CompanyId,IDNP,JobTitle,Status,Id")] Representative representative)
        {
            if (id != representative.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(representative);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RepresentativeExists(representative.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Name", representative.CompanyId);
            return View(representative);
        }

        [Authorize(Roles = UserRoles.Admin)]
        // GET: Representatives/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var representative = await _context.Representatives
                .Include(r => r.Company)
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (representative == null)
            {
                return NotFound();
            }

            return View(representative);
        }

        [Authorize(Roles = UserRoles.Admin)]
        // POST: Representatives/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var representative = await _context.Representatives.FindAsync(id);
            _context.Representatives.Remove(representative);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RepresentativeExists(int id)
        {
            return _context.Representatives.Any(e => e.Id == id);
        }
    }
}
