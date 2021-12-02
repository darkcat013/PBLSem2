using Construx.App.Constants;
using Construx.App.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Construx.App.Data.Seeds
{
    public class PlanPartStatusesSeed
    {
        public static async Task Seed(ApplicationDbContext dbContext)
        {
            var statuses = dbContext.Set<PlanPartStatus>();
            if (!statuses.Any())
            {
                List<PlanPartStatus> statusesList = new List<PlanPartStatus>
                {
                    new PlanPartStatus
                    {
                        Name = PlanPartStatuses.Active
                    },
                    new PlanPartStatus
                    {
                        Name = PlanPartStatuses.Finished
                    },
                    new PlanPartStatus
                    {
                        Name = PlanPartStatuses.InProgress
                    },
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
