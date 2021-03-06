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
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace Construx.App.Controllers
{
    public class CompaniesController : BaseController
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IGenericRepository<City> _cityRepository;
        private readonly IGenericRepository<CompanyStatus> _companyStatusRepository;
        private readonly IRepresentativeRepository _representativeRepository;
        private readonly UserManager<User> _userManager;
        private readonly IGenericRepository<Bookmark> _bookmarkRepository;
        private readonly IGenericRepository<Category> _categoryRepository;
        private readonly IReviewRepository _reviewRepository;
        private readonly IServiceRepository _serviceRepository;
        private readonly IPhotoRepository _photoRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public CompaniesController(ICompanyRepository companyRepository, IGenericRepository<City> cityRepository, UserManager<User> userManager, IGenericRepository<CompanyStatus> companyStatusRepository, IRepresentativeRepository representativeRepository, IGenericRepository<Bookmark> bookmarkRepository, IGenericRepository<Category> categoryRepository, IReviewRepository reviewRepository, IServiceRepository serviceRepository, IPhotoRepository photoRepository, IWebHostEnvironment webHostEnvironment)
        {
            _companyRepository = companyRepository;
            _cityRepository = cityRepository;
            _userManager = userManager;
            _companyStatusRepository = companyStatusRepository;
            _representativeRepository = representativeRepository;
            _bookmarkRepository = bookmarkRepository;
            _categoryRepository = categoryRepository;
            _reviewRepository = reviewRepository;
            _serviceRepository = serviceRepository;
            _photoRepository = photoRepository;
            _webHostEnvironment = webHostEnvironment;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index(string searchString, string sortOrder, string sortCity)
        {
            ViewData["getSearchString"] = searchString;
            ViewData["getSortCity"] = sortCity;
            ViewData["getSortOrder"] = sortOrder;

            IEnumerable<Company> companies = await _companyRepository.GetApprovedCompanies();

            if (!String.IsNullOrEmpty(searchString))
            {
                companies = companies.Where(c => c.Name.ToLower().Contains(searchString.ToLower()) || c.Description.ToLower().Contains(searchString.ToLower()));
            }

            if (!String.IsNullOrEmpty(sortCity))
            {
                companies = companies.Where(c => c.City.Name.Equals(sortCity));
            }

            switch (sortOrder)
            {
                case "nameASC":
                    companies = companies.OrderBy(c => c.Name);
                    break;

                case "nameDESC":
                    companies = companies.OrderByDescending(c => c.Name);
                    break;
            }
            ViewData["Cities"] = await _cityRepository.GetAll();

            return View(companies.ToList());
        }

        public async Task<IActionResult> BookmarkCompany(string id)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            Bookmark bookmarkCompany = new()
            {
                UserId = user.Id,
                CompanyId = Convert.ToInt32(id)
            };
            _bookmarkRepository.Add(bookmarkCompany);
            await _bookmarkRepository.SaveChangesAsync();

            return LocalRedirect("~/Companies/Details/" + id);
        }

        [AllowAnonymous]
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
            ViewData["HasBookmark"] = company.Bookmarks.FirstOrDefault(b => b.User.UserName == User.Identity.Name) != null;
            IEnumerable<Photo> photo = new List<Photo>(await _photoRepository.GetPhotoByCompanyId(id.Value));
            ViewBag.HasPhoto = photo.Any();
            ViewBag.photo = photo;

            return View(company);
        }

        [Authorize(Roles = UserRoles.Admin + "," + UserRoles.Representative)]
        public async Task<IActionResult> Create()
        {
            if (User.IsInRole(UserRoles.Representative))
            {
                var representative = await _representativeRepository.GetByUserName(User.Identity.Name);
                if (representative.CompanyId == null)
                {
                    var company = new Company { StatusId = (int)StatusesIds.NeedsDetails, CityId = 1 };
                    _companyRepository.Add(company);
                    await _companyRepository.SaveChangesAsync();
                    representative.CompanyId = company.Id;
                    await _representativeRepository.SaveChangesAsync();
                }
                else
                {
                    return LocalRedirect("~/");
                }
            }
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
            if (ModelState.IsValid)
            {
                if (User.IsInRole(UserRoles.Admin))
                {
                    var company = createCompanyDto.Adapt<Company>();
                    company.StatusId = (int)StatusesIds.Approved;
                    _companyRepository.Add(company);
                    await _companyRepository.SaveChangesAsync();
                }
                else if (User.IsInRole(UserRoles.Representative))
                {
                    var representative = await _representativeRepository.GetByUserName(User.Identity.Name);
                    if (representative.CompanyId != null)
                    {
                        var company = await _companyRepository.GetById(representative.CompanyId.Value);
                        company.StatusId = (int)StatusesIds.UnderVerification;
                        company.Name = createCompanyDto.Name;
                        company.Adress = createCompanyDto.Adress;
                        company.Phone = createCompanyDto.Phone;
                        company.Email = createCompanyDto.Email;
                        company.IDNO = createCompanyDto.IDNO;
                        company.Website = createCompanyDto.Website;
                        company.Description = createCompanyDto.Description;
                        company.CityId = createCompanyDto.CityId;
                        await _companyRepository.SaveChangesAsync();
                    }
                }

                return RedirectToAction(nameof(Index));
            }
            ViewData["CityId"] = new SelectList(await _cityRepository.GetAll(), "Id", "Name");
            return View(createCompanyDto);
        }

        [Authorize(Roles = UserRoles.Admin + "," + UserRoles.Representative)]
        public async Task<IActionResult> Edit(int? id, IFormFile file)
        {
            var company = await _companyRepository.GetById(id.Value);

            if (id == null || ((company.Representative == null || company.Representative.User.UserName != User.Identity.Name) && !User.IsInRole(UserRoles.Admin)))
            {
                return NotFound();
            }

            if (company == null)
            {
                return NotFound();
            }
            ViewData["CityId"] = new SelectList(await _cityRepository.GetAll(), "Id", "Name", company.Name);
            ViewData["StatusId"] = new SelectList(await _companyStatusRepository.GetAll(), "Id", "Name", company.Name);
            ViewData["Categories"] = new SelectList(await _categoryRepository.GetAll(), "Id", "Name");
            return View(company);
        }

        [Authorize(Roles = UserRoles.Admin + "," + UserRoles.Representative)]
        // POST: Companies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Name,Adress,Phone,Email,IDNO,Website,StatusId,Description,RepresentativeId,CityId,Id")] Company company, IFormFile file)
        {
            if (id != company.Id)
            {
                return NotFound();
            }

            if (file != null)
            {
                var currUser = await _userManager.FindByNameAsync(User.Identity.Name);
                var currPhoto = await _photoRepository.GetPhotoByCompanyId(id);
                if (currPhoto.Any())
                {
                    await DeletePhoto(currPhoto[0].Id);
                }

                await CreateAndUploadPhoto(ObjectTypes.Company, file, (int)ObjectTypesIds.Company, currUser.Id, id);
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _companyRepository.Update(company);
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
                return LocalRedirect("~/Companies/Details/" + id);
            }
            ViewData["CityId"] = new SelectList(await _cityRepository.GetAll(), "Id", "Name", company.Name);
            ViewData["StatusId"] = new SelectList(await _companyStatusRepository.GetAll(), "Id", "Name", company.Name);
            ViewData["Categories"] = new SelectList(await _categoryRepository.GetAll(), "Id", "Name");
            return View(company);
        }

        [Authorize(Roles = UserRoles.Admin + "," + UserRoles.Representative)]
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
            var company = await _companyRepository.GetById(id);
            if (company.Representative != null)
            {
                await _companyRepository.Delete(id);
                await _companyRepository.SaveChangesAsync();
            }
            else
            {
                company.Representative.CompanyId = null;
                await _companyRepository.SaveChangesAsync();
                await _companyRepository.Delete(id);
                await _companyRepository.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = UserRoles.Admin + "," + UserRoles.Representative)]
        [HttpPost, ActionName("CreateService")]
        public async Task<IActionResult> CreateService(int id, int category, string name, string description)
        {
            Service service = new Service
            {
                CompanyId = id,
                CategoryId = category,
                Name = name,
                Description = description
            };
            _serviceRepository.Add(service);
            await _serviceRepository.SaveChangesAsync();

            return LocalRedirect($"~/Companies/Details/{id}");
        }

        private bool CompanyExists(int id)
        {
            return _companyRepository.GetById(id) != null;
        }

        public async Task CreateAndUploadPhoto(string folder, IFormFile file, int objectTypeId, int userId, int objectId)
        {
            string wwwRootPath = _webHostEnvironment.WebRootPath;
            string filename = "/uploaded/" + folder + "/" + Path.GetFileNameWithoutExtension(file.FileName) + DateTime.Now.Ticks + Path.GetExtension(file.FileName);
            string path = wwwRootPath + filename;

            var photo = new Photo
            {
                Name = filename,
                ObjectTypeId = objectTypeId,
                ObjectId = objectId,
                UserId = userId
            };

            using (var fileStream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }
            _photoRepository.Add(photo);
            await _photoRepository.SaveChangesAsync();
        }

        public async Task DeletePhoto(int id)
        {
            var photo = await _photoRepository.GetById(id);

            string wwwRootPath = _webHostEnvironment.WebRootPath;
            var path = wwwRootPath + photo.Name;

            if (System.IO.File.Exists(path))
                System.IO.File.Delete(path);
            await _photoRepository.Delete(id);
            await _photoRepository.SaveChangesAsync();
        }
    }
}