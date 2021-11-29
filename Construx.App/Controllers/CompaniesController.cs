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
using Construx.App.Dtos.Company;
using Mapster;
using MediatR;
using Construx.App.Interfaces;
using Microsoft.AspNetCore.Identity;
using Construx.App.Domain.Identity;

namespace Construx.App.Controllers
{
    public class CompaniesController : BaseController
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IGenericRepository<City> _cityRepository;
        private readonly IGenericRepository<CompanyStatus> _companyStatusRepository;
        private readonly UserManager<User> _userManager;

        public CompaniesController(ICompanyRepository companyRepository, IGenericRepository<City> cityRepository, UserManager<User> userManager, IGenericRepository<CompanyStatus> companyStatusRepository)
        {
            _companyRepository = companyRepository;
            _cityRepository = cityRepository;
            _userManager = userManager;
            _companyStatusRepository = companyStatusRepository;
        }

        [AllowAnonymous]
        // GET: Companies
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _companyRepository.GetAll();
            return View(await applicationDbContext);
        }

        [AllowAnonymous]
        // GET: Companies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var company = await _companyRepository.GetById(id.Value);
            if (company == null)
            {
                return NotFound();
            }

            return View(company);
        }

        
        [Authorize(Roles = UserRoles.Admin + "," + UserRoles.Representative)]
        // GET: Companies/Create
        public async Task<IActionResult> Create()
        {
            ViewData["CityId"] = new SelectList(await _cityRepository.GetAll(), "Id", "Name");
            return View();
        }


        [Authorize(Roles = UserRoles.Admin + "," + UserRoles.Representative)]
        // POST: Companies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Adress,Phone,Email,IDNO,Website,Description,CityId")] CreateCompanyDto createCompanyDto)
        {
            var company = createCompanyDto.Adapt<Company>();
            if (ModelState.IsValid)
            {
                if(User.IsInRole(UserRoles.Admin))
                {
                    company.StatusId = (int)StatusesIds.Approved;
                }
                else if(User.IsInRole(UserRoles.Representative))
                {
                    company.StatusId = (int)StatusesIds.UnderVerification;
                    company.RepresentativeId = (await _userManager.FindByNameAsync(User.Identity.Name)).Id;
                }
                _companyRepository.Add(company);
                await _companyRepository.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CityId"] = new SelectList(await _cityRepository.GetAll(), "Id", "Name", company.Name);
            return View(company);
        }

        [Authorize(Roles = UserRoles.Admin + "," + UserRoles.Representative)]
        // GET: Companies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var company = await _companyRepository.GetById(id.Value);
            if (company == null)
            {
                return NotFound();
            }
            ViewData["CityId"] = new SelectList(await _cityRepository.GetAll(), "Id", "Name", company.Name);
            ViewData["StatusId"] = new SelectList(await _companyStatusRepository.GetAll(), "Id", "Name", company.Name);
            return View(company);
        }

        [Authorize(Roles = UserRoles.Admin + "," + UserRoles.Representative)]
        // POST: Companies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Name,Adress,Phone,Email,IDNO,Website,StatusId,Description,RepresentativeId,CityId,Id")] Company company)
        {
            if (id != company.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _companyRepository.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CompanyExists(company.Id))
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
            ViewData["CityId"] = new SelectList(await _cityRepository.GetAll(), "Id", "Name", company.Name);
            ViewData["StatusId"] = new SelectList(await _companyStatusRepository.GetAll(), "Id", "Name", company.Name);
            return View(company);
        }

        [Authorize(Roles = UserRoles.Admin + "," + UserRoles.Representative)]
        // GET: Companies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var company = await _companyRepository.GetById(id.Value);
            if (company == null)
            {
                return NotFound();
            }

            return View(company);
        }

        [Authorize(Roles = UserRoles.Admin + "," + UserRoles.Representative)]
        // POST: Companies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _companyRepository.Delete(id);
            await _companyRepository.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CompanyExists(int id)
        {
            return _companyRepository.GetById(id) != null;
        }
    }
}
