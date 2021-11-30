using Construx.App.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Construx.App.Data.Seeds
{
    public class CategoriesSeed
    {
        public static async Task Seed(ApplicationDbContext dbContext)
        {
            var categories = dbContext.Set<Category>();
            if (!categories.Any())
            {
                List<Category> categoriesList = new List<Category>
                {
                    new Category
                    {
                        Name = "Roofs"
                    },
                    new Category
                    {
                        Name = "Cleaning"
                    },
                    new Category
                    {
                        Name = "Interior decorations"
                    },
                    new Category
                    {
                        Name = "Construction materials"
                    },
                    new Category
                    {
                        Name = "Wood constructions"
                    },
                    new Category
                    {
                        Name = "Electric installations"
                    },
                    new Category
                    {
                        Name = "Wall painting"
                    },
                    new Category
                    {
                        Name = "Construction equipment"
                    },
                    new Category
                    {
                        Name = "Doors"
                    },
                    new Category
                    {
                        Name = "Windows"
                    }
                };
                foreach (var item in categoriesList)
                {
                    await categories.AddAsync(item);
                    await dbContext.SaveChangesAsync();
                }
            }
        }
    }
}
