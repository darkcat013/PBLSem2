using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Construx.App.Data;
using Construx.App.Domain.Entities;

namespace Construx.App.Controllers
{
    public class BookmarkServicesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BookmarkServicesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: BookmarkServices
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.BookmarkServices.Include(b => b.Service).Include(b => b.User);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: BookmarkServices/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookmarkService = await _context.BookmarkServices
                .Include(b => b.Service)
                .Include(b => b.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bookmarkService == null)
            {
                return NotFound();
            }

            return View(bookmarkService);
        }

        // GET: BookmarkServices/Create
        public IActionResult Create()
        {
            ViewData["ServiceId"] = new SelectList(_context.Services, "Id", "Id");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: BookmarkServices/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ServiceId,UserId,Note,DateCreated,Id")] BookmarkService bookmarkService)
        {
            if (ModelState.IsValid)
            {
                _context.Add(bookmarkService);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ServiceId"] = new SelectList(_context.Services, "Id", "Id", bookmarkService.ServiceId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", bookmarkService.UserId);
            return View(bookmarkService);
        }

        // GET: BookmarkServices/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookmarkService = await _context.BookmarkServices.FindAsync(id);
            if (bookmarkService == null)
            {
                return NotFound();
            }
            ViewData["ServiceId"] = new SelectList(_context.Services, "Id", "Id", bookmarkService.ServiceId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", bookmarkService.UserId);
            return View(bookmarkService);
        }

        // POST: BookmarkServices/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ServiceId,UserId,Note,DateCreated,Id")] BookmarkService bookmarkService)
        {
            if (id != bookmarkService.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bookmarkService);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookmarkServiceExists(bookmarkService.Id))
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
            ViewData["ServiceId"] = new SelectList(_context.Services, "Id", "Id", bookmarkService.ServiceId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", bookmarkService.UserId);
            return View(bookmarkService);
        }

        // GET: BookmarkServices/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookmarkService = await _context.BookmarkServices
                .Include(b => b.Service)
                .Include(b => b.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bookmarkService == null)
            {
                return NotFound();
            }

            return View(bookmarkService);
        }

        // POST: BookmarkServices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var bookmarkService = await _context.BookmarkServices.FindAsync(id);
            _context.BookmarkServices.Remove(bookmarkService);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookmarkServiceExists(int id)
        {
            return _context.BookmarkServices.Any(e => e.Id == id);
        }
    }
}
