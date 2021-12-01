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
    public class ServicesController : Controller
    {
        private readonly IGenericRepository<Category> _categoryRepository;
        private readonly IServiceRepository _serviceRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly UserManager<User> _userManager;
        private readonly IPlanPartRepository _planPartRepository;
        private readonly IPlanRepository _planRepository;

        public ServicesController(IGenericRepository<Category> categoryRepository, IServiceRepository serviceRepository, ICompanyRepository companyRepository, UserManager<User> userManager, IPlanPartRepository planPartRepository, IPlanRepository planRepository)
        {
            _categoryRepository = categoryRepository;
            _serviceRepository = serviceRepository;
            _companyRepository = companyRepository;
            _userManager = userManager;
            _planPartRepository = planPartRepository;
            _planRepository = planRepository;
        }

        // GET: Services
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

        // GET: Services/Details/5
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

            return View(service);
        }

        // GET: Services/Create
        public async Task<IActionResult> Create()
        {
            ViewData["CategoryId"] = new SelectList(await _categoryRepository.GetAll(), "Id", "Name");
            ViewData["CompanyId"] = new SelectList(await _companyRepository.GetAll(), "Id", "Name");
            return View();
        }

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

        // GET: Services/Edit/5
        public async Task<IActionResult> Edit(int? id)
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
            ViewData["CategoryId"] = new SelectList(await _categoryRepository.GetAll(), "Id", "Name");
            ViewData["CompanyId"] = new SelectList(await _companyRepository.GetAll(), "Id", "Name");
            return View(service);
        }

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

        // GET: Services/Delete/5
        public async Task<IActionResult> Delete(int? id)
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

            return View(service);
        }

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
    }
}