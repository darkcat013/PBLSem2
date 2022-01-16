using Construx.App.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Construx.App.Interfaces
{
    public interface IServiceRepository : IGenericRepository<Service>
    {
        public Task<List<Service>> GetServicesByCompanyId(int CompanyId);
    }
}