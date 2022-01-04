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
    public class ReviewRepository : GenericRepository<Review>, IReviewRepository
    {
        private readonly DbSet<Review> _reviews;
        private readonly UserManager<User> _userManager;

        public ReviewRepository(ApplicationDbContext applicationDbContext, UserManager<User> userManager) : base(applicationDbContext)
        {
            _reviews = applicationDbContext.Set<Review>();
            _userManager = userManager;
        }

        public async Task<List<Review>> GetReviewsByCompanyId(int CompanyId)
        {
            var reviews = _reviews.Where(r => r.Service.CompanyId == CompanyId);
            return await reviews.ToListAsync();
        }

        public async Task<List<Review>> GetReviewsByServiceId(int serviceId)
        {
            var reviews = _reviews.Where(r => r.ServiceId == serviceId);
            return await reviews.ToListAsync();
        }
    }
}