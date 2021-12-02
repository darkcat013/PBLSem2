using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Construx.App.Data;
using Construx.App.Domain.Entities;
using Construx.App.Interfaces;

namespace Construx.App.Controllers
{
    public class BookmarkCompaniesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IBookmarkCompaniesRepository _bookmarkCompaniesRepository;

        public BookmarkCompaniesController(ApplicationDbContext context, IBookmarkCompaniesRepository bookmarkCompaniesRepository)
        {
            _context = context;
            _bookmarkCompaniesRepository = bookmarkCompaniesRepository;
        }

        // GET: BookmarkCompanies
        public async Task<IActionResult> Index()
        {
            var bookmarksCompanies = await _bookmarkCompaniesRepository.GetBookmarksCompanyForUserName(User.Identity.Name);
            return View(bookmarksCompanies);
        }

        // GET: BookmarkCompanies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookmarkCompany = await _context.BookmarkCompanies
                .Include(b => b.Company)
                .Include(b => b.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bookmarkCompany == null)
            {
                return NotFound();
            }

            return View(bookmarkCompany);
        }

        // GET: BookmarkCompanies/Create
        public IActionResult Create()
        {
            ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Name");
            ViewData["UserId"] = new SelectList(_context.Users.Where(u => u.UserName.Equals(User.Identity.Name)), "Id", "UserName");
            return View();
        }

        // POST: BookmarkCompanies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CompanyId,UserId,Note,DateCreated,Id")] BookmarkCompany bookmarkCompany)
        {
            if (ModelState.IsValid)
            {
                _context.Add(bookmarkCompany);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Id", bookmarkCompany.CompanyId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", bookmarkCompany.UserId);
            return View(bookmarkCompany);
        }

        // GET: BookmarkCompanies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookmarkCompany = await _context.BookmarkCompanies.FindAsync(id);
            if (bookmarkCompany == null)
            {
                return NotFound();
            }
            ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Id", bookmarkCompany.CompanyId);
            ViewData["UserId"] = new SelectList(_context.Users.Where(u => u.UserName.Equals(User.Identity.Name)), "Id", "UserName");
            return View(bookmarkCompany);
        }

        // POST: BookmarkCompanies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CompanyId,UserId,Note,DateCreated,Id")] BookmarkCompany bookmarkCompany)
        {
            if (id != bookmarkCompany.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bookmarkCompany);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookmarkCompanyExists(bookmarkCompany.Id))
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
            ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Id", bookmarkCompany.CompanyId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", bookmarkCompany.UserId);
            return View(bookmarkCompany);
        }

        // GET: BookmarkCompanies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookmarkCompany = await _context.BookmarkCompanies
                .Include(b => b.Company)
                .Include(b => b.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bookmarkCompany == null)
            {
                return NotFound();
            }

            return View(bookmarkCompany);
        }

        // POST: BookmarkCompanies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var bookmarkCompany = await _context.BookmarkCompanies.FindAsync(id);
            _context.BookmarkCompanies.Remove(bookmarkCompany);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookmarkCompanyExists(int id)
        {
            return _context.BookmarkCompanies.Any(e => e.Id == id);
        }
    }
}