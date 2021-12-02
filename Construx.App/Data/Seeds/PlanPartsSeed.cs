using Construx.App.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Construx.App.Data.Seeds
{
    public class PlanPartsSeed
    {
        public static async Task Seed(ApplicationDbContext dbContext)
        {
            var planParts = dbContext.PlanParts;
            if (!planParts.Any())
            {
                List<PlanPart> planPartsList = new List<PlanPart>
                {
                    new PlanPart()
                    {
                        Name = "Repair holes",
                        Description = " ",
                        FromDate = DateTime.Now,
                        ToDate = DateTime.Now.AddDays(1),
                        StatusId = 3,
                        PlanId = 1,
                        ServiceId = 1,
                        Priority = 1,
                    },
                    new PlanPart()
                    {
                        Name = "Clean roof",
                        Description = " ",
                        FromDate = DateTime.Now,
                        ToDate = DateTime.Now.AddDays(2),
                        StatusId = 1,
                        PlanId = 1,
                        ServiceId = 2,
                        Priority = 2,
                    },
                    new PlanPart()
                    {
                        Name = "Indoor decorations",
                        Description = "decoration for mirror, for living",
                        FromDate = DateTime.Now.AddDays(3),
                        ToDate = DateTime.Now.AddDays(5),
                        StatusId = 1,
                        PlanId = 2,
                        ServiceId = 4,
                        Priority = 5,
                    },
                    new PlanPart()
                    {
                        Name = "Iron decorations",
                        Description = " ",
                        FromDate = DateTime.Now.AddDays(4),
                        ToDate = DateTime.Now.AddDays(7),
                        StatusId = 3,
                        PlanId = 2,
                        Priority = 6,
                    },
                };
                foreach (var planPart in planPartsList)
                {
                    await planParts.AddAsync(planPart);
                    await dbContext.SaveChangesAsync();
                }
            }
        }
    }
}
