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
    public class BookmarkCompaniesController : Controller
    {
        private readonly IBookmarkCompaniesRepository _bookmarkCompaniesRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly UserManager<User> _userManager;
        public BookmarkCompaniesController(IBookmarkCompaniesRepository bookmarkCompaniesRepository, ICompanyRepository companyRepository, UserManager<User> userManager)
        {
            _bookmarkCompaniesRepository = bookmarkCompaniesRepository;
            _companyRepository = companyRepository;
            _userManager = userManager;
        }

        // GET: BookmarkCompanies
        public async Task<IActionResult> Index()
        {
            var bookmarksCompanies = await _bookmarkCompaniesRepository.GetBookmarksCompanyForUserName(User.Identity.Name);
            return View(bookmarksCompanies);
        }

        // GET: BookmarkCompanies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookmarkCompany = await _bookmarkCompaniesRepository.GetById(id.Value);

            if (bookmarkCompany == null)
            {
                return NotFound();
            }

            return View(bookmarkCompany);
        }

        // GET: BookmarkCompanies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookmarkCompany = await _bookmarkCompaniesRepository.GetById(id.Value);
            if (bookmarkCompany == null)
            {
                return NotFound();
            }

            return View(bookmarkCompany);
        }

        // POST: BookmarkCompanies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _bookmarkCompaniesRepository.Delete(id);
            await _bookmarkCompaniesRepository.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookmarkCompanyExists(int id)
        {
            return _bookmarkCompaniesRepository.GetById(id) != null;
        }
    }
}