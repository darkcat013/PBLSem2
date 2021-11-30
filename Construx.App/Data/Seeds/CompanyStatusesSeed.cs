using Construx.App.Constants;
using Construx.App.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Construx.App.Data.Seeds
{
    public class CompanyStatusesSeed
    {
        public static async Task Seed(ApplicationDbContext dbContext)
        {
            var statuses = dbContext.Set<CompanyStatus>();
            if (!statuses.Any())
            {
                List<CompanyStatus> statusesList = new List<CompanyStatus>
                {
                    new CompanyStatus
                    {
                        Name = Statuses.Approved
                    },
                    new CompanyStatus
                    {
                        Name = Statuses.NotActive
                    },
                    new CompanyStatus
                    {
                        Name = Statuses.UnderVerification
                    },
                    new CompanyStatus
                    {
                        Name = Statuses.NeedsDetails
                    }
                };
                foreach (var item in statusesList)
                {
                    await statuses.AddAsync(item);
                    await dbContext.SaveChangesAsync();
                }
            }
        }
    }
}
