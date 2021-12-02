using Construx.App.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Construx.App.Interfaces
{
    public interface ICompanyRepository : IGenericRepository<Company>
    {
        public Task<List<Company>> GetApprovedCompanies();
    }
}
