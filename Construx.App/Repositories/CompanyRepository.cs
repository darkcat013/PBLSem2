using Construx.App.Data;
using Construx.App.Domain.Entities;
using Construx.App.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Construx.App.Repositories
{
    public class CompanyRepository :  GenericRepository<Company>, ICompanyRepository
    {
        private readonly DbSet<Company> _companies;
        public CompanyRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
            _companies = applicationDbContext.Set<Company>();
        }
    }
}
