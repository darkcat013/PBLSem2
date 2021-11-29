using Construx.App.Constants;
using Construx.App.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Construx.App.Data.Seeds
{
    public static class UsersSeed
    {
        public static async Task Seed(UserManager<User> userManager)
        {
            if (!userManager.Users.Any())
            {
                List<User> users = new List<User>
                {
                    new User
                    {
                        UserName = "darkcat013",
                        Email = "viorel.noroc013@gmail.com",
                        FirstName = "Viorel",
                        LastName = "Noroc",
                        DateCreated = new DateTime(2021,9,1),
                        CityId = 14
                    },
                    new User
                    {
                        UserName = "bonehair",
                        Email = "faraamk@gmail.com",
                        CityId = 14
                    },
                    new User
                    {
                        UserName = "user3",
                        Email = "user3@gmail.com",
                        CityId = 1
                    },
                    new User
                    {
                        UserName = "user4",
                        Email = "user4@gmail.com",
                        CityId = 2
                    },
                };
                await userManager.CreateAsync(users[0], "a12345678");
                await userManager.AddToRoleAsync(users[0], UserRoles.Admin);

                await userManager.CreateAsync(users[1], "a12345678");
                await userManager.AddToRoleAsync(users[1], UserRoles.User);

                await userManager.CreateAsync(users[2], "a12345678");
                await userManager.AddToRoleAsync(users[2], UserRoles.User);

                await userManager.CreateAsync(users[3], "a12345678");
                await userManager.AddToRoleAsync(users[3], UserRoles.User);

            }
        }
    }
}
