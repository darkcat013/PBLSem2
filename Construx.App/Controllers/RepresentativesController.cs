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
    public class RepresentativesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RepresentativesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Representatives
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Representatives.Include(r => r.Company).Include(r => r.User);
            return View(await applicationDbContext.ToListAsync());
        }

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
        public IActionResult Create()
        {
            ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Id");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: Representatives/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,CompanyId,IDNP,Phone,JobTitle,Id")] Representative representative)
        {
            if (ModelState.IsValid)
            {
                _context.Add(representative);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Id", representative.CompanyId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", representative.UserId);
            return View(representative);
        }

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
            ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Id", representative.CompanyId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", representative.UserId);
            return View(representative);
        }

        // POST: Representatives/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserId,CompanyId,IDNP,Phone,JobTitle,Id")] Representative representative)
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
            ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Id", representative.CompanyId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", representative.UserId);
            return View(representative);
        }

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
