using Construx.App.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Construx.App.Data.Seeds
{
    public class PlansSeed
    {
        public static async Task Seed(ApplicationDbContext dbContext)
        {
            var plans = dbContext.Plans;
            if (!plans.Any())
            {
                List<Plan> plansList = new List<Plan>
                {
                    new Plan()
                    {
                        UserId = 2,
                        Name = "Roof repair",
                        Description = "Roof is messy and with holes"
                    },
                    new Plan()
                    {
                        UserId=2,
                        Name = "Indoor decorations",
                        Description = "Need some nice decorations"
                    },
                };
                foreach (var plan in plansList)
                {
                    await plans.AddAsync(plan);
                    await dbContext.SaveChangesAsync();
                }
            }
        }
    }
}
