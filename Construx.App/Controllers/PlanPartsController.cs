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
    public class PlanPartsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PlanPartsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: PlanParts
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.PlanParts.Include(p => p.Plan).Include(p => p.Service).Include(p => p.Status);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: PlanParts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var planPart = await _context.PlanParts
                .Include(p => p.Plan)
                .Include(p => p.Service)
                .Include(p => p.Status)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (planPart == null)
            {
                return NotFound();
            }

            return View(planPart);
        }

        // GET: PlanParts/Create
        public IActionResult Create()
        {
            ViewData["PlanId"] = new SelectList(_context.Plans, "Id", "Id");
            ViewData["ServiceId"] = new SelectList(_context.Services, "Id", "Id");
            ViewData["StatusId"] = new SelectList(_context.PlanPartsStatuses, "Id", "Id");
            return View();
        }

        // POST: PlanParts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Description,FromDate,ToDate,StatusId,PlanId,ServiceId,Priority,Id")] PlanPart planPart)
        {
            if (ModelState.IsValid)
            {
                _context.Add(planPart);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PlanId"] = new SelectList(_context.Plans, "Id", "Id", planPart.PlanId);
            ViewData["ServiceId"] = new SelectList(_context.Services, "Id", "Id", planPart.ServiceId);
            ViewData["StatusId"] = new SelectList(_context.PlanPartsStatuses, "Id", "Id", planPart.StatusId);
            return View(planPart);
        }

        // GET: PlanParts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var planPart = await _context.PlanParts.FindAsync(id);
            if (planPart == null)
            {
                return NotFound();
            }
            ViewData["PlanId"] = new SelectList(_context.Plans, "Id", "Id", planPart.PlanId);
            ViewData["ServiceId"] = new SelectList(_context.Services, "Id", "Id", planPart.ServiceId);
            ViewData["StatusId"] = new SelectList(_context.PlanPartsStatuses, "Id", "Id", planPart.StatusId);
            return View(planPart);
        }

        // POST: PlanParts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Name,Description,FromDate,ToDate,StatusId,PlanId,ServiceId,Priority,Id")] PlanPart planPart)
        {
            if (id != planPart.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(planPart);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PlanPartExists(planPart.Id))
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
            ViewData["PlanId"] = new SelectList(_context.Plans, "Id", "Id", planPart.PlanId);
            ViewData["ServiceId"] = new SelectList(_context.Services, "Id", "Id", planPart.ServiceId);
            ViewData["StatusId"] = new SelectList(_context.PlanPartsStatuses, "Id", "Id", planPart.StatusId);
            return View(planPart);
        }

        // GET: PlanParts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var planPart = await _context.PlanParts
                .Include(p => p.Plan)
                .Include(p => p.Service)
                .Include(p => p.Status)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (planPart == null)
            {
                return NotFound();
            }

            return View(planPart);
        }

        // POST: PlanParts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var planPart = await _context.PlanParts.FindAsync(id);
            _context.PlanParts.Remove(planPart);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PlanPartExists(int id)
        {
            return _context.PlanParts.Any(e => e.Id == id);
        }
    }
}
