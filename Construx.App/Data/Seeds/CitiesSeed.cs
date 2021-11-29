using Construx.App.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Construx.App.Data.Seeds
{
    public class CitiesSeed
    {
        public static async Task Seed(ApplicationDbContext dbContext)
        {
            var cities = dbContext.Set<City>();
            if (cities.Any())
            {
                return;
            }
            List<City> citiesList = new List<City>
                {
                    new City
                    {
                        Name = "Anenii Noi"
                    },
                    new City
                    {
                        Name = "Balti"
                    },
                    new City
                    {
                        Name = "Basarabeasca"
                    },
                    new City
                    {
                        Name = "Biruinta"
                    },
                    new City
                    {
                        Name = "Briceni"
                    },
                    new City
                    {
                        Name = "Bucovat"
                    },
                    new City
                    {
                        Name = "Cainari"
                    },
                    new City
                    {
                        Name = "Calarasi"
                    },
                    new City
                    {
                        Name = "Causeni"
                    },
                    new City
                    {
                        Name = "Cahul"
                    },
                    new City
                    {
                        Name = "Camenca"
                    },
                    new City
                    {
                        Name = "Cantemir"
                    },
                    new City
                    {
                        Name = "Ceadar-Lunga"
                    },
                    new City
                    {
                        Name = "Chisinau"
                    },
                    new City
                    {
                        Name = "Cimislia"
                    },
                    new City
                    {
                        Name = "Codru"
                    },
                    new City
                    {
                        Name = "Comrat"
                    },
                    new City
                    {
                        Name = "Cornesti"
                    },
                    new City
                    {
                        Name = "Costesti"
                    },
                    new City
                    {
                        Name = "Crasnoe"
                    },
                    new City
                    {
                        Name = "Cricova"
                    },
                    new City
                    {
                        Name = "Criuleni"
                    },
                    new City
                    {
                        Name = "Cupcini"
                    },
                    new City
                    {
                        Name = "Dnestrovsc"
                    },
                    new City
                    {
                        Name = "Donduseni"
                    },
                    new City
                    {
                        Name = "Drochia"
                    },
                    new City
                    {
                        Name = "Dubasari"
                    },
                    new City
                    {
                        Name = "Durlesti"
                    },
                    new City
                    {
                        Name = "Edinet"
                    },
                    new City
                    {
                        Name = "Falesti"
                    },
                    new City
                    {
                        Name = "Floresti"
                    },
                    new City
                    {
                        Name = "Frunza"
                    },
                    new City
                    {
                        Name = "Ghindesti"
                    },
                    new City
                    {
                        Name = "Glodeni"
                    },
                    new City
                    {
                        Name = "Grigoriopol"
                    },
                    new City
                    {
                        Name = "Hancesti"
                    },
                    new City
                    {
                        Name = "Ialoveni"
                    },
                    new City
                    {
                        Name = "Iargara"
                    },
                    new City
                    {
                        Name = "Leova"
                    },
                    new City
                    {
                        Name = "Lipcani"
                    },
                    new City
                    {
                        Name = "Marculesti"
                    },
                    new City
                    {
                        Name = "Maiac"
                    },
                    new City
                    {
                        Name = "Nisporeni"
                    },
                    new City
                    {
                        Name = "Ocnita"
                    },
                    new City
                    {
                        Name = "Orhei"
                    },
                    new City
                    {
                        Name = "Otaci"
                    },
                    new City
                    {
                        Name = "Rascani"
                    },
                    new City
                    {
                        Name = "Rezina"
                    },
                    new City
                    {
                        Name = "Ribnita"
                    },
                    new City
                    {
                        Name = "Sangera"
                    },
                    new City
                    {
                        Name = "Sangerei"
                    },
                    new City
                    {
                        Name = "Slobozia"
                    },
                    new City
                    {
                        Name = "Soroca"
                    },
                    new City
                    {
                        Name = "Soldanesti"
                    },
                    new City
                    {
                        Name = "Stefan Voda"
                    },
                    new City
                    {
                        Name = "Straseni"
                    },
                    new City
                    {
                        Name = "Taraclia"
                    },
                    new City
                    {
                        Name = "Telenesti"
                    },
                    new City
                    {
                        Name = "Tighina"
                    },
                    new City
                    {
                        Name = "Tiraspol"
                    },
                    new City
                    {
                        Name = "Tiraspolul Nou"
                    },
                    new City
                    {
                        Name = "Tvardita"
                    },
                    new City
                    {
                        Name = "Ungheni"
                    },
                    new City
                    {
                        Name = "Vadul lui Voda"
                    },
                    new City
                    {
                        Name = "Vatra"
                    },
                    new City
                    {
                        Name = "Vulcanesti"
                    },

                };
            foreach (var item in citiesList)
            {
                await cities.AddAsync(item);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
