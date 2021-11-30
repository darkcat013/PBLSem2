using Construx.App.Data;
using Construx.App.Domain.Entities;
using Construx.App.Domain.Identity;
using Construx.App.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Construx.App.Repositories
{
    public class RepresentativeRepository : GenericRepository<Representative>, IRepresentativeRepository
    {
        private readonly DbSet<Representative> _representatives;
        private readonly UserManager<User> _userManager;
        public RepresentativeRepository(ApplicationDbContext applicationDbContext, UserManager<User> userManager) : base(applicationDbContext)
        {
            _representatives = applicationDbContext.Set<Representative>();
            _userManager = userManager;
        }

        public async Task<Representative> GetByUserId(int userId)
        {
            var representative = await _representatives.FirstOrDefaultAsync(r=>r.UserId == userId);
            return representative;
        }

        public async Task<Representative> GetByUserName(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            var representative = await GetByUserId(user.Id);
            return representative;
        }
    }
}
