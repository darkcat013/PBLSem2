using Construx.App.Data;
using Construx.App.Domain.Entities;
using Construx.App.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Construx.App.Repositories
{
    public class PlanPartRepository : GenericRepository<PlanPart>, IPlanPartRepository
    {
        private readonly DbSet<PlanPart> _planParts;
        public PlanPartRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
            _planParts = applicationDbContext.Set<PlanPart>();
        }

        public async Task<List<PlanPart>> GetPlanPartsForPlanId(int planId)
        {
            var planParts = _planParts.Where(pp => pp.PlanId == planId);
            return await planParts.ToListAsync();
        }
    }
}
