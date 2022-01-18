using Construx.App.Data;
using Construx.App.Domain.Entities;
using Construx.App.Domain.Identity;
using Construx.App.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Construx.App.Constants;

namespace Construx.App.Repositories
{
    public class PhotoRepository : GenericRepository<Photo>, IPhotoRepository
    {
        private readonly DbSet<Photo> _photos;
        private readonly UserManager<User> _userManager;

        public PhotoRepository(ApplicationDbContext applicationDbContext, UserManager<User> userManager) : base(applicationDbContext)
        {
            _photos = applicationDbContext.Set<Photo>();
            _userManager = userManager;
        }

        public async Task<List<Photo>> GetPhotoByCompanyId(int companyId)
        {
            var photo = _photos.Where(p => p.ObjectType.Name == ObjectTypes.Company && p.ObjectId == companyId);

            return await photo.ToListAsync();
        }

        public async Task<List<Photo>> GetPhotosByPlanPartId(int ppId)
        {
            var photos = _photos.Where(p => p.ObjectType.Name == ObjectTypes.PlanPart && p.ObjectId == ppId);

            return await photos.ToListAsync();
        }

        public async Task<List<Photo>> GetPhotosByRepresentativeId(int repId)
        {
            var photos = _photos.Where(p => p.ObjectType.Name == ObjectTypes.Representative && p.ObjectId == repId);

            return await photos.ToListAsync();
        }

        public async Task<List<Photo>> GetPhotosByReviewId(int reviewId)
        {
            var photos = _photos.Where(p => p.ObjectType.Name == ObjectTypes.Review && p.ObjectId == reviewId);

            return await photos.ToListAsync();
        }
    }
}