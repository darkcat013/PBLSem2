using Construx.App.Data;
using Construx.App.Domain.Entities;
using Construx.App.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Construx.App.Repositories
{
    public class ServiceRepository : GenericRepository<Service>, IServiceRepository
    {
        private readonly DbSet<Service> _services;
        public ServiceRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
            _services = applicationDbContext.Set<Service>();
        }
    }
}
