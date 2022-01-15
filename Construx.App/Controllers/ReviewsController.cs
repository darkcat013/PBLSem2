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
using Construx.App.Constants;
using Microsoft.AspNetCore.Authorization;

namespace Construx.App.Controllers
{
    public class ReviewsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IReviewRepository _reviewRepository;
        private readonly UserManager<User> _userManager;
        private readonly IServiceRepository _serviceRepository;

        public ReviewsController(ApplicationDbContext context, IReviewRepository reviewRepository, IServiceRepository serviceRepository, UserManager<User> userManager)
        {
            _context = context;
            _reviewRepository = reviewRepository;
            _userManager = userManager;
            _serviceRepository = serviceRepository;
        }

        // GET: Reviews
        public async Task<IActionResult> Index()
        {
            var reviews = await _reviewRepository.GetReviewsByUserName(User.Identity.Name);
            return View(reviews);
        }

        // GET: Reviews/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var review = await _context.Reviews
                .Include(r => r.Service)
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (review == null)
            {
                return NotFound();
            }

            return View(review);
        }

        // GET: Reviews/Create
        public async Task<IActionResult> Create()
        {
            ViewData["ServiceId"] = new SelectList(await _serviceRepository.GetAll(), "Id", "Name");
            ViewData["UserId"] = (await _userManager.FindByNameAsync(User.Identity.Name)).Id;
            return View();
        }

        // POST: Reviews/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,ServiceId,Rating,Description,Id")] Review review)
        {
            if (ModelState.IsValid)
            {
                _context.Add(review);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ServiceId"] = new SelectList(await _serviceRepository.GetAll(), "Id", "Name");
            ViewData["UserId"] = (await _userManager.FindByNameAsync(User.Identity.Name)).Id;
            return View(review);
        }

        // GET: Reviews/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var review = await _context.Reviews.FindAsync(id);
            if (review == null)
            {
                return NotFound();
            }
            ViewData["ServiceId"] = new SelectList(await _serviceRepository.GetAll(), "Id", "Name");
            ViewData["UserId"] = (await _userManager.FindByNameAsync(User.Identity.Name)).Id;
            return View(review);
        }

        // POST: Reviews/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserId,ServiceId,Rating,Description,Id")] Review review)
        {
            if (id != review.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(review);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReviewExists(review.Id))
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
            ViewData["ServiceId"] = new SelectList(await _serviceRepository.GetAll(), "Id", "Name");
            ViewData["UserId"] = (await _userManager.FindByNameAsync(User.Identity.Name)).Id;
            return View(review);
        }

        // GET: Reviews/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var review = await _context.Reviews
                .Include(r => r.Service)
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (review == null)
            {
                return NotFound();
            }

            return View(review);
        }

        // POST: Reviews/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var review = await _context.Reviews.FindAsync(id);
            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReviewExists(int id)
        {
            return _context.Reviews.Any(e => e.Id == id);
        }
    }
}