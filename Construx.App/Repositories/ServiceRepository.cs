using Construx.App.Data;
using Construx.App.Domain.Entities;
using Construx.App.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Construx.App.Repositories
{
    public class ServiceRepository : GenericRepository<Service>, IServiceRepository
    {
        private readonly DbSet<Service> _services;

        public ServiceRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
            _services = applicationDbContext.Set<Service>();
        }

        public async Task<List<Service>> GetServicesByCompanyId(int CompanyId)
        {
            var services = _services.Where(s => s.CompanyId == CompanyId);

            return await services.ToListAsync();
        }
    }
}