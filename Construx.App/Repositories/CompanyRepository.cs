using Construx.App.Data;
using Construx.App.Domain.Entities;
using Construx.App.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Construx.App.Repositories
{
    public class CompanyRepository :  GenericRepository<Company>, ICompanyRepository
    {
        private readonly DbSet<Company> _companies;
        public CompanyRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
            _companies = applicationDbContext.Set<Company>();
        }

        public async Task<List<Company>> GetApprovedCompanies()
        {
            var companies = _companies.Where(c=>c.StatusId == 1 || c.StatusId == 2);
            return await companies.ToListAsync();
        }
    }
}
