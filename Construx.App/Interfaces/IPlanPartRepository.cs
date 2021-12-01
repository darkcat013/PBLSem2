using Construx.App.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Construx.App.Interfaces
{
    public interface IPlanPartRepository : IGenericRepository<PlanPart>
    {
        public Task<List<PlanPart>> GetPlanPartsForPlanId(int planId);
    }
}
