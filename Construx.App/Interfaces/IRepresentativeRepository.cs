using Construx.App.Domain.Entities;
using System.Threading.Tasks;

namespace Construx.App.Interfaces
{
    public interface IRepresentativeRepository : IGenericRepository<Representative>
    {
        public Task<Representative> GetByUserId(int id);
        public Task<Representative> GetByUserName(string userName);
    }
}
