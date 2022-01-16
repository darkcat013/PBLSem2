using Construx.App.Data;
using Construx.App.Domain.Entities;
using Construx.App.Domain.Identity;
using Construx.App.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace Construx.App.Repositories
{
    public class BookmarkRepository : GenericRepository<Bookmark>, IBookmarkRepository
    {
        private readonly DbSet<Bookmark> _bookmarksCompany;
        private readonly UserManager<User> _userManager;

        public BookmarkRepository(ApplicationDbContext applicationDbContext, UserManager<User> userManager) : base(applicationDbContext)
        {
            _bookmarksCompany = applicationDbContext.Set<Bookmark>();
            _userManager = userManager;
        }

        public Task<List<Bookmark>> GetBookmarksForUserName(string userName)
        {
            var bookmarksCompany = _bookmarksCompany.Where(bc => bc.User.UserName == userName);
            return bookmarksCompany.ToListAsync();
        }
    }
}