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
using Construx.App.Constants;
using Microsoft.AspNetCore.Authorization;

namespace Construx.App.Controllers
{
    public class PlanPartsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IPlanRepository _planRepository;
        private readonly IServiceRepository _serviceRepository;
        private readonly IPlanPartRepository _planPartRepository;

        public PlanPartsController(ApplicationDbContext context, IPlanRepository planRepository, IPlanPartRepository planPartRepository, IServiceRepository serviceRepository)
        {
            _context = context;
            _planRepository = planRepository;
            _planPartRepository = planPartRepository;
            _serviceRepository = serviceRepository;
        }

        [Authorize(Roles = UserRoles.User)]
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

        [Authorize(Roles = UserRoles.User)]
        // GET: PlanParts/Create
        public async Task<IActionResult> Create(string planid, string serviceid)
        {
            if (string.IsNullOrEmpty(planid))
            {
                ViewData["Plan"] = new SelectList(await _planRepository.GetAll(), "Id", "Name");
            }
            else
            {
                ViewData["Plan"] = new SelectList(new List<Plan> { await _planRepository.GetById(Convert.ToInt32(planid)) }, "Id", "Name");
            }
            if (!string.IsNullOrEmpty(serviceid))
            {
                ViewData["Service"] = new SelectList(new List<Service> { await _serviceRepository.GetById(Convert.ToInt32(serviceid)) }, "Id", "Name");
            }
            else ViewData["Service"] = null;
            ViewData["Status"] = new SelectList(_context.PlanPartsStatuses, "Id", "Name");
            return View();
        }

        // POST: PlanParts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = UserRoles.User)]
        public async Task<IActionResult> Create([Bind("Name,Description,FromDate,ToDate,StatusId,PlanId,ServiceId,Priority,Id")] PlanPart planPart, string planid, string serviceid)
        {
            if (ModelState.IsValid)
            {
                _context.Add(planPart);
                await _context.SaveChangesAsync();
                if (string.IsNullOrEmpty(serviceid))
                {
                    return LocalRedirect("~/Services/Index");
                }
                else
                {
                    return LocalRedirect($"~/Plans/Details/{planid}");
                }
            }
            if (string.IsNullOrEmpty(planid))
            {
                ViewData["Plan"] = new SelectList(await _planRepository.GetAll(), "Id", "Name");
            }
            else
            {
                ViewData["Plan"] = new SelectList(new List<Plan> { await _planRepository.GetById(Convert.ToInt32(planid)) }, "Id", "Name");
            }
            if (!string.IsNullOrEmpty(serviceid))
            {
                ViewData["Service"] = new SelectList(new List<Service> { await _serviceRepository.GetById(Convert.ToInt32(serviceid)) }, "Id", "Name");
            }
            else ViewData["Service"] = null;
            ViewData["Status"] = new SelectList(_context.PlanPartsStatuses, "Id", "Name");
            return View(planPart);
        }

        // GET: PlanParts/Edit/5
        [Authorize(Roles = UserRoles.User)]
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
            ViewData["PlanId"] = new SelectList(_context.Plans, "Id", "Name");
            ViewData["ServiceId"] = new SelectList(_context.Services, "Id", "Name");
            ViewData["StatusId"] = new SelectList(_context.PlanPartsStatuses, "Id", "Name");
            return View(planPart);
        }

        // POST: PlanParts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = UserRoles.User)]
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
                return LocalRedirect($"~/Plans/Details/{planPart.PlanId}");
            }
            ViewData["PlanId"] = new SelectList(_context.Plans, "Id", "Name");
            ViewData["ServiceId"] = new SelectList(_context.Services, "Id", "Name");
            ViewData["StatusId"] = new SelectList(_context.PlanPartsStatuses, "Id", "Name");
            return View(planPart);
        }

        // GET: PlanParts/Delete/5
        [Authorize(Roles = UserRoles.User)]
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
        [Authorize(Roles = UserRoles.User)]
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
        [Authorize(Roles = UserRoles.User)]
        public async Task<IActionResult> ChangeService(int serviceId, int planPartId)
        {
            if (planPartId != 0)
            {
                var planPart = await _planPartRepository.GetById(planPartId);
                planPart.ServiceId = serviceId;
                await _planPartRepository.SaveChangesAsync();
                return LocalRedirect($"~/PlanParts/Details/{planPartId}");
            }
            return LocalRedirect($"~/Services/Details/{serviceId}");
        }
    }
}