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
using Microsoft.AspNetCore.Authorization;
using Construx.App.Constants;

namespace Construx.App.Controllers
{
    public class ServicesController : Controller
    {
        private readonly IGenericRepository<Category> _categoryRepository;
        private readonly IServiceRepository _serviceRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly UserManager<User> _userManager;
        private readonly IPlanPartRepository _planPartRepository;
        private readonly IPlanRepository _planRepository;
        private readonly IReviewRepository _reviewRepository;
        private readonly IBookmarkRepository _bookmarkRepository;

        public ServicesController(IGenericRepository<Category> categoryRepository, IServiceRepository serviceRepository, ICompanyRepository companyRepository, UserManager<User> userManager, IPlanPartRepository planPartRepository, IPlanRepository planRepository, IReviewRepository reviewRepository, IBookmarkRepository bookmarkRepository)
        {
            _categoryRepository = categoryRepository;
            _serviceRepository = serviceRepository;
            _companyRepository = companyRepository;
            _userManager = userManager;
            _planPartRepository = planPartRepository;
            _planRepository = planRepository;
            _reviewRepository = reviewRepository;
            _bookmarkRepository = bookmarkRepository;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index(string sortCategory, string searchString)
        {
            ViewData["getSortCategory"] = sortCategory;
            ViewData["getSearchString"] = searchString;
            ViewData["Categories"] = await _categoryRepository.GetAll();

            IEnumerable<Service> services = await _serviceRepository.GetAll();

            if (!String.IsNullOrEmpty(searchString))
            {
                services = services.Where(s => s.Name.ToLower().Contains(searchString.ToLower()) || s.Description.ToLower().Contains(searchString.ToLower()));
            }

            if (!String.IsNullOrEmpty(sortCategory))
            {
                services = services.Where(s => s.Category.Name.Equals(sortCategory));
            }

            return View(services.ToList());
        }

        public async Task<IActionResult> BookmarkService(int id)
        {
            var service = await _serviceRepository.GetById(id);

            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            Bookmark bookmark = new()
            {
                CompanyId = service.CompanyId,
                UserId = user.Id,
                ServiceId = id
            };
            _bookmarkRepository.Add(bookmark);
            await _bookmarkRepository.SaveChangesAsync();

            return LocalRedirect("~/Services/Details/" + id);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var service = await _serviceRepository.GetById(id.Value);
            if (service == null)
            {
                return NotFound();
            }
            ViewData["Plans"] = new SelectList(await _planRepository.GetPlansForUserName(User.Identity.Name), "Id", "Name");
            ViewBag.HasReview = service.Reviews.FirstOrDefault(r => r.User.UserName == User.Identity.Name) != null;
            ViewBag.HasBookmark = service.Bookmarks.FirstOrDefault(b => b.User.UserName == User.Identity.Name) != null;
            return View(service);
        }

        public async Task<IActionResult> AddReview(int serviceId, int rating, string description)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            Review review = new Review
            {
                ServiceId = serviceId,
                Rating = rating,
                Description = description,
                UserId = user.Id
            };

            if (rating <= 5 && rating >= 1) _reviewRepository.Add(review);
            await _reviewRepository.SaveChangesAsync();

            return LocalRedirect("~/Services/Details/" + serviceId);
        }

        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> Create()
        {
            ViewData["CategoryId"] = new SelectList(await _categoryRepository.GetAll(), "Id", "Name");
            ViewData["CompanyId"] = new SelectList(await _companyRepository.GetAll(), "Id", "Name");
            return View();
        }

        [Authorize(Roles = UserRoles.Admin)]
        // POST: Services/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CompanyId,CategoryId,Name,Description,Id")] Service service)
        {
            if (ModelState.IsValid)
            {
                _serviceRepository.Add(service);
                await _serviceRepository.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(await _categoryRepository.GetAll(), "Id", "Name");
            ViewData["CompanyId"] = new SelectList(await _companyRepository.GetAll(), "Id", "Name");
            return View(service);
        }

        [Authorize(Roles = UserRoles.Admin + "," + UserRoles.Representative)]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var service = await _serviceRepository.GetById(id.Value);
            if (service == null || (service.Company.Representative.User.UserName != User.Identity.Name && !User.IsInRole(UserRoles.Admin)))
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(await _categoryRepository.GetAll(), "Id", "Name");
            ViewData["CompanyId"] = new SelectList(await _companyRepository.GetAll(), "Id", "Name");
            return View(service);
        }

        [Authorize(Roles = UserRoles.Admin + "," + UserRoles.Representative)]
        // POST: Services/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CompanyId,CategoryId,Name,Description,Id")] Service service)
        {
            if (id != service.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _serviceRepository.Update(service);
                    await _serviceRepository.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!(await ServiceExists(service.Id)))
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
            ViewData["CategoryId"] = new SelectList(await _categoryRepository.GetAll(), "Id", "Name");
            ViewData["CompanyId"] = new SelectList(await _companyRepository.GetAll(), "Id", "Name");
            return View(service);
        }

        [Authorize(Roles = UserRoles.Admin + "," + UserRoles.Representative)]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var service = await _serviceRepository.GetById(id.Value);
            if (service == null || (service.Company.Representative.User.UserName != User.Identity.Name && !User.IsInRole(UserRoles.Admin)))
            {
                return NotFound();
            }

            return View(service);
        }

        [Authorize(Roles = UserRoles.Admin + "," + UserRoles.Representative)]
        // POST: Services/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _serviceRepository.Delete(id);
            await _serviceRepository.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> ServiceExists(int id)
        {
            return (await _serviceRepository.GetById(id)) != null;
        }

        [Authorize(Roles = UserRoles.User)]
        [ActionName("AddToPlan")]
        public async Task<IActionResult> AddToPlan(int serviceid, int plan)
        {
            ViewData["Service"] = await _serviceRepository.GetById(serviceid);
            ViewData["Plan"] = await _planRepository.GetById(plan);
            ViewData["EmptyPlanParts"] = new SelectList(await _planPartRepository.GetPlanPartsWithoutServiceForPlanId(plan), "Id", "Name");
            ViewData["PlanParts"] = new SelectList(await _planPartRepository.GetPlanPartsWithServiceForPlanId(plan), "Id", "Name");
            return View();
        }
    }
}