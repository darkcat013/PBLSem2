using Construx.App.Data;
using Construx.App.Data.Seeds;
using Construx.App.Domain.Entities;
using Construx.App.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Construx.App.Extensions
{
    public static class HostExtensions
    {
        public static async Task SeedData(this IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var dbContext = services.GetRequiredService<ApplicationDbContext>();

                    var roleManager = services.GetRequiredService<RoleManager<Role>>();
                    var userManager = services.GetRequiredService<UserManager<User>>();

                    dbContext.Database.Migrate();

                    await CitiesSeed.Seed(dbContext);
                    await CategoriesSeed.Seed(dbContext);
                    await RolesSeed.Seed(roleManager);
                    await UsersSeed.Seed(userManager);
                    await CompanyStatusesSeed.Seed(dbContext);
                    await CompaniesSeed.Seed(dbContext);
                    await PlanPartStatusesSeed.Seed(dbContext);
                    await ServicesSeed.Seed(dbContext);
                    await RepresentativesSeed.Seed(dbContext);
                    await PlansSeed.Seed(dbContext);
                    await PlanPartsSeed.Seed(dbContext);
                    await ObjectTypesSeed.Seed(dbContext);
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occured during migration");
                }
            }
        }
    }
}