using Construx.App.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Construx.App.Data.Seeds
{
    public class ServicesSeed
    {
        public static async Task Seed(ApplicationDbContext dbContext)
        {
            var services = dbContext.Services;
            if(!services.Any())
            {
                List<Service> serviceList = new List<Service>
                {
                    new Service()
                    {
                        CategoryId = 1,
                    }
                };
                foreach(var service in services)
                {
                    await services.AddAsync(service);
                    await dbContext.SaveChangesAsync();
                }    
            }
        }
    }
}
