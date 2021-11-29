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

namespace Construx.App.Controllers
{
    public class RepresentativesController : BaseController
    {
        private readonly ApplicationDbContext _context;

        public RepresentativesController(ApplicationDbContext context)
        {
            _context = context;
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
        public IActionResult Create()
        {
            return View();
        }

        // POST: Representatives/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IDNP,JobTitle,Status,Id")] Representative representative)
        {
            if (ModelState.IsValid)
            {
                _context.Add(representative);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(representative);
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
