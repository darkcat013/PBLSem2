using Construx.App.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Construx.App.Interfaces
{
    public interface IPhotoRepository : IGenericRepository<Photo>
    {
        public Task<List<Photo>> GetPhotoByCompanyId(int companyId);

        public Task<List<Photo>> GetPhotosByRepresentativeId(int repId);

        public Task<List<Photo>> GetPhotosByReviewId(int reviewId);

        public Task<List<Photo>> GetPhotosByPlanPartId(int ppId);
    }
}