using Construx.App.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Construx.App.Interfaces
{
    public interface IReviewRepository : IGenericRepository<Review>
    {
        public Task<List<Review>> GetReviewsByServiceId(int serviceId);

        public Task<List<Review>> GetReviewsByCompanyId(int CompanyId);
    }
}