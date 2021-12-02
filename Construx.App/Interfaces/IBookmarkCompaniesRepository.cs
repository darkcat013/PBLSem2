using Construx.App.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Construx.App.Interfaces
{
    public interface IBookmarkCompaniesRepository : IGenericRepository<BookmarkCompany>
    {
        public Task<List<BookmarkCompany>> GetBookmarksCompanyForUserName(string userName);
    }
}