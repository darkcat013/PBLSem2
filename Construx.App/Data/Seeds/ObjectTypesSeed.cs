using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Construx.App.Domain.Entities;

namespace Construx.App.Data.Seeds
{
    public class ObjectTypesSeed
    {
        public static async Task Seed(ApplicationDbContext dbContext)
        {
            var objectTypes = dbContext.Set<ObjectType>();

            if (!objectTypes.Any())
            {
                List<ObjectType> objectTypesList = new()
                {
                    new ObjectType
                    {
                        Name = "Company"
                    },
                    new ObjectType
                    {
                        Name = "Review"
                    },
                    new ObjectType
                    {
                        Name = "Plan"
                    },
                    new ObjectType
                    {
                        Name = "Representative"
                    }
                };

                foreach (var item in objectTypesList)
                {
                    await objectTypes.AddAsync(item);
                    await dbContext.SaveChangesAsync();
                }
            }
        }
    }
}