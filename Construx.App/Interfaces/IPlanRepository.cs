using Construx.App.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Construx.App.Interfaces
{
    public interface IPlanRepository : IGenericRepository<Plan>
    {
        public Task<List<Plan>> GetPlansForUserName(string userName);
    }
}
