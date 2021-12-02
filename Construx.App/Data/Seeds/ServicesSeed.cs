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
                        CompanyId = 1,
                        CategoryId = 1,
                        Name = "Hidroizolare acoperis",
                        Description = "Hidroizolarea acoperisurilor"
                    },
                    new Service()
                    {
                        CompanyId = 1,
                        CategoryId = 1,
                        Name = "Coinstruire acoperis",
                        Description = "Construirea unui acoperis nou pentru casa dumneavoastra"
                    },
                    new Service()
                    {
                        CompanyId = 1,
                        CategoryId = 2,
                        Name = "Curatire acoperis",
                        Description = " "
                    },
                    new Service()
                    {
                        CompanyId = 4,
                        CategoryId = 3,
                        Name = "Decoratii interioare",
                        Description = " "
                    },
                    new Service()
                    {
                        CompanyId = 4,
                        CategoryId = 4,
                        Name = "Materiale de constructii",
                        Description = "Oferim orice material aveti nevoie"
                    },
                    new Service()
                    {
                        CompanyId = 4,
                        CategoryId = 5,
                        Name = "Constructii din lemn",
                        Description = " "
                    },
                    new Service()
                    {
                        CompanyId = 4,
                        CategoryId = 8,
                        Name = "Echipament constructii",
                        Description = " "
                    },
                    new Service()
                    {
                        CompanyId = 4,
                        CategoryId = 8,
                        Name = "Leasing echipament constructii",
                        Description = " "
                    },
                };
                foreach(var service in serviceList)
                {
                    await services.AddAsync(service);
                    await dbContext.SaveChangesAsync();
                }    
            }
        }
    }
}
