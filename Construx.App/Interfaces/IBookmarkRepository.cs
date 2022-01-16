using Construx.App.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Construx.App.Interfaces
{
    public interface IBookmarkRepository : IGenericRepository<Bookmark>
    {
        public Task<List<Bookmark>> GetBookmarksForUserName(string userName);
    }
}