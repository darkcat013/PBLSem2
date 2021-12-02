using Construx.App.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Construx.App.Data.Seeds
{
    public class RepresentativesSeed
    {
        public static async Task Seed(ApplicationDbContext dbContext)
        {
            var representatives = dbContext.Representatives;
            if (!representatives.Any())
            {
                List<Representative> representativesList = new List<Representative>
                {
                    new Representative()
                    {
                        CompanyId = 1,
                        UserId = 3,
                        IDNP = "2004009043810",
                        JobTitle = "Director",
                        Status = true
                    },
                    new Representative()
                    {
                        CompanyId = 3,
                        UserId = 4,
                        IDNP = "2004009043811",
                        JobTitle = "Director",
                        Status = true
                    },
                };
                foreach (var representative in representativesList)
                {
                    await representatives.AddAsync(representative);
                    await dbContext.SaveChangesAsync();
                }
            }
        }
    }
}
