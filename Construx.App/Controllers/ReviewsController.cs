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
        private readonly ICompanyRepository _companyRepository;

        public ReviewsController(ApplicationDbContext context, IReviewRepository reviewRepository, IServiceRepository serviceRepository, UserManager<User> userManager, ICompanyRepository companyRepository)
        {
            _context = context;
            _reviewRepository = reviewRepository;
            _userManager = userManager;
            _serviceRepository = serviceRepository;
            _companyRepository = companyRepository;
        }

        // GET: Reviews
        public async Task<IActionResult> Index()
        {
            var reviews = await _reviewRepository.GetReviewsByUserName(User.Identity.Name);
            return View(reviews);
        }

        // GET: Reviews/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var review = await _context.Reviews.FindAsync(id);
            if (review == null || review.User.UserName != User.Identity.Name)
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
                    var service = await _serviceRepository.GetById(review.ServiceId);
                    service.Rating = (await _reviewRepository.GetReviewsByServiceId(service.Id)).Where(x => x.Rating > 0).Average(x => x.Rating);
                    await _serviceRepository.SaveChangesAsync();
                    var company = await _companyRepository.GetById(service.CompanyId);
                    company.Rating = (await _serviceRepository.GetServicesByCompanyId(company.Id)).Where(x => x.Rating > 0).Average(x => x.Rating);
                    await _companyRepository.SaveChangesAsync();
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
            if (review == null || review.User.UserName != User.Identity.Name)
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
            int serviceId = review.ServiceId;
            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();
            var service = await _serviceRepository.GetById(serviceId);
            service.Rating = (await _reviewRepository.GetReviewsByServiceId(service.Id)).Where(x => x.Rating > 0).Select(x => x.Rating).DefaultIfEmpty(0).Average();
            await _serviceRepository.SaveChangesAsync();
            var company = await _companyRepository.GetById(service.CompanyId);
            company.Rating = (await _serviceRepository.GetServicesByCompanyId(company.Id)).Where(x => x.Rating > 0).Select(x => x.Rating).DefaultIfEmpty(0).Average();
            await _companyRepository.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReviewExists(int id)
        {
            return _context.Reviews.Any(e => e.Id == id);
        }
    }
}