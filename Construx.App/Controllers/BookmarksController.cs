using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Construx.App.Data;
using Construx.App.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Construx.App.Interfaces;
using Construx.App.Domain.Identity;

namespace Construx.App.Controllers
{
    public class BookmarksController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly IBookmarkRepository _bookmarkRepository;
        private readonly IServiceRepository _serviceRepository;

        public BookmarksController(UserManager<User> userManager, IBookmarkRepository bookmarkRepository, IServiceRepository serviceRepository)
        {
            _userManager = userManager;
            _bookmarkRepository = bookmarkRepository;
            _serviceRepository = serviceRepository;
        }

        // GET: Bookmarks
        public async Task<IActionResult> Index()
        {
            var bookmarks = await _bookmarkRepository.GetBookmarksForUserName(User.Identity.Name);
            return View(bookmarks);
        }

        // GET: Bookmarks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookmark = await _bookmarkRepository.GetById(id.Value);

            if (bookmark == null || bookmark.User.UserName != User.Identity.Name)
            {
                return NotFound();
            }

            return View(bookmark);
        }

        // GET: Bookmarks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var bookmark = await _bookmarkRepository.GetById(id.Value);
            ViewBag.ServiceId = new SelectList(await _serviceRepository.GetServicesByCompanyId(bookmark.CompanyId), "Id", "Name");

            if (id == null)
            {
                return NotFound();
            }

            if (bookmark == null || bookmark.User.UserName != User.Identity.Name)
            {
                return NotFound();
            }
            return View(bookmark);
        }

        // POST: Bookmarks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CompanyId,ServiceId,UserId,Note,DateCreated,Id")] Bookmark bookmark)
        {
            if (id != bookmark.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _bookmarkRepository.Update(bookmark);
                    await _bookmarkRepository.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookmarkExists(bookmark.Id))
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

            return View(bookmark);
        }

        // GET: Bookmarks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookmark = await _bookmarkRepository.GetById(id.Value);
            if (bookmark == null || bookmark.User.UserName != User.Identity.Name)
            {
                return NotFound();
            }

            return View(bookmark);
        }

        // POST: Bookmarks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var bookmark = await _bookmarkRepository.GetById(id);
            await _bookmarkRepository.Delete(id);
            await _bookmarkRepository.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool BookmarkExists(int id)
        {
            return _bookmarkRepository.GetById(id) != null;
        }
    }
}