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
    public class PlanRepository : GenericRepository<Plan>, IPlanRepository
    {
        private readonly DbSet<Plan> _plans;
        private readonly UserManager<User> _userManager;
        public PlanRepository(ApplicationDbContext applicationDbContext, UserManager<User> userManager) : base(applicationDbContext)
        {
            _plans = applicationDbContext.Set<Plan>();
            _userManager = userManager;
        }

        public Task<List<Plan>> GetPlansForUserName(string userName)
        {
            var plans = _plans.Where(p => p.User.UserName == userName);
            return plans.ToListAsync();
        }
    }
}
