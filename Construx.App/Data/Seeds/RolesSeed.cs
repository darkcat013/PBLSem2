using Construx.App.Constants;
using Construx.App.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Construx.App.Data.Seeds
{
    public static class RolesSeed
    {
        public static async Task Seed(RoleManager<Role> roleManager)
        {
            if(!roleManager.Roles.Any())
            {
                List<Role> roles = new List<Role>
                {
                    new Role
                    {
                        Name = UserRoles.Admin
                    },
                    new Role
                    {
                        Name = UserRoles.User
                    },
                    new Role
                    {
                        Name = UserRoles.Representative
                    }
                };
                await roleManager.CreateAsync(roles[0]);
                await roleManager.CreateAsync(roles[1]);
                await roleManager.CreateAsync(roles[2]);
            }
        }
    }
}
