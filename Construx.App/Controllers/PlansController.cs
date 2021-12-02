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
using Microsoft.AspNetCore.Identity;
using Construx.App.Domain.Identity;

namespace Construx.App.Controllers
{
    public class PlansController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IPlanRepository _planRepository;
        private readonly IPlanPartRepository _planPartRepository;
        private readonly UserManager<User> _userManager;
        private readonly IGenericRepository<PlanPartStatus> _planPartStatusRepository;

        public PlansController(ApplicationDbContext context, IPlanRepository planRepository, IPlanPartRepository planPartRepository, UserManager<User> userManager, IGenericRepository<PlanPartStatus> planPartStatusRepository)
        {
            _context = context;
            _planRepository = planRepository;
            _planPartRepository = planPartRepository;
            _userManager = userManager;
            _planPartStatusRepository = planPartStatusRepository;
        }

        // GET: Plans
        public async Task<IActionResult> Index()
        {
            var plans = await _planRepository.GetPlansForUserName(User.Identity.Name);
            return View(plans);
        }

        // GET: Plans/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var plan = await _context.Plans
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (plan == null)
            {
                return NotFound();
            }
            ViewData["PlanParts"] = await _planPartRepository.GetPlanPartsForPlanId(id.Value);
            return View(plan);
        }

        // GET: Plans/Create
        public async Task<IActionResult> Create()
        {
            ViewData["UserId"] = (await _userManager.FindByNameAsync(User.Identity.Name)).Id;
            return View();
        }

        // POST: Plans/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,Name,Description")] Plan plan)
        {
            if (ModelState.IsValid)
            {
                _context.Add(plan);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = (await _userManager.FindByNameAsync(User.Identity.Name)).Id;
            return View(plan);
        }

        // GET: Plans/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            ViewData["PlanPartStatuses"] = new SelectList(await _planPartStatusRepository.GetAll(), "Id", "Name");

            if (id == null)
            {
                return NotFound();
            }

            var plan = await _context.Plans.FindAsync(id);
            if (plan == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Users.Where(u => u.UserName.Equals(User.Identity.Name)), "Id", "UserName");
            return View(plan);
        }

        // POST: Plans/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserId,Name,Description,Id")] Plan plan)
        {
            ViewData["PlanPartStatuses"] = new SelectList(await _planPartStatusRepository.GetAll(), "Id", "Name");

            if (id != plan.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(plan);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PlanExists(plan.Id))
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
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", plan.UserId);
            return View(plan);
        }

        [HttpPost, ActionName("CreatePlanPart")]
        public async Task<IActionResult> CreatePlanPart(int id, string name, string description, DateTime fromDate, DateTime toDate, int priority, int planPartStatusId)
        {
            PlanPart planPart = new PlanPart
            {
                PlanId = id,
                Name = name,
                Description = description,
                FromDate = fromDate,
                ToDate = toDate,
                Priority = priority,
                StatusId = planPartStatusId
            };

            _planPartRepository.Add(planPart);
            await _planPartRepository.SaveChangesAsync();

            return LocalRedirect($"~/Plans/Details/{id}");
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var plan = await _context.Plans
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (plan == null)
            {
                return NotFound();
            }

            return View(plan);
        }

        // POST: Plans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var plan = await _context.Plans.FindAsync(id);
            _context.Plans.Remove(plan);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PlanExists(int id)
        {
            return _context.Plans.Any(e => e.Id == id);
        }
    }
}