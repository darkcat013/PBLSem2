using Construx.App.Constants;
using Construx.App.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Construx.App.Data.Seeds
{
    public class CompaniesSeed
    {
        public static async Task Seed(ApplicationDbContext dbContext)
        {
            var companies = dbContext.Set<Company>();
            if (!companies.Any())
            {
                List<Company> companiesList = new List<Company>
                {
                    new Company
                    {
                        Adress = "str. Lupu Vasile, 44",
                        CityId = 9,
                        Description = "Intreprinzator individual Tigaie Petru",
                        Email = "",
                        IDNO = "1017608001927",
                        Name = "ÎNTREPRINZĂTOR INDIVIDUAL ŢIGAIE PETRU",
                        Phone = "+37368243534",
                        StatusId = (int)StatusesIds.Approved
                    },
                    new Company
                    {
                        Adress = "str. test",
                        CityId = 9,
                        Description = "test desc",
                        Email = "amdaris@test.com",
                        IDNO = "100000000",
                        Name = "TEST",
                        Phone = "+3736866866",
                        StatusId = (int)StatusesIds.UnderVerification,
                        Website = "https://example.com/"
                    },
                    new Company
                    {
                        Adress = "str. Florilor",
                        CityId = 55,
                        Description = "Lista fondatorilor GODOROJA VASILE (10.0%), KONONOV SERGIY (60.0%), KONONOVA OLGA (20.0%), RYBALCHENKO ANDRII (10.0%),",
                        Email = "equipmentsupport@gmail.com",
                        IDNO = "1011600017070",
                        Name = "SOCIETATEA CU RĂSPUNDERE LIMITATĂ EQUIPMENT-SUPPORT",
                        Phone = "+37367431353",
                        StatusId = (int)StatusesIds.NotActive,
                    },
                    new Company
                    {
                        Adress = "str. Asachi Gh., 62/5, ap.(of.) 4A",
                        CityId = 14,
                        Description = "Lista fondatorilor CERCHEZ ALEXANDRU (35.0%), CERCHEZ VASILE (35.0%), CERCHEZ VITALIE (30.0%),",
                        Email = "luxprodus@gmail.com",
                        IDNO = "1009600016126",
                        Name = "SOCIETATEA CU RĂSPUNDERE LIMITATĂ LUXPRODUS",
                        Phone = "+37379038441",
                        StatusId = (int)StatusesIds.Approved,
                        Website = "https://example.com/"
                    }
                };
                foreach (var item in companiesList)
                {
                    await companies.AddAsync(item);
                    await dbContext.SaveChangesAsync();
                }
            }
        }
    }
}
