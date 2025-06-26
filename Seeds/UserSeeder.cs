using BookStoresWebAPI.Models;
using BookStoresWebAPI.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BookStoresWebAPI.Seeds
{
    public class UserSeeder
    {
        
    public static async Task SeedAdminAsync(IServiceProvider serviceProvider, ILoggerFactory loggerFactory)
        {
            var logger = loggerFactory.CreateLogger("UserSeeder");

            var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>(); //hardcoding admin infos
            string adminEmail = "kadriyeh26@gmail.com";
            string adminUserName = "Kadriye";
            string adminPassword = "1234";

            var existingUser = await userManager.FindByNameAsync(adminUserName);
            if (existingUser != null)
            {
                logger.LogInformation("Admin user already exists: {UserName}", adminUserName);
                var isInRole = await userManager.IsInRoleAsync(existingUser, "Admin"); //check if the auth of admin has done
                if (!isInRole)
                {
                    await userManager.AddToRoleAsync(existingUser, "Admin");
                    logger.LogInformation("Admin role was missing and added.");
                }

                return;
            }

            
                var newUser = new AppUser
                {
                    UserName = adminUserName,
                    Email = adminEmail,
                    EmailConfirmed = true
                };
                var result = await userManager.CreateAsync(newUser, "adminPassword"); //kullanıcıya yetki ataması

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(newUser, "Admin");
                    logger.LogInformation("Admin user created and added to role.");
                }
                else
                {
                    logger.LogError("Admin user creation failed: {Errors}", string.Join(", ", result.Errors.Select(e => e.Description)));
                }

            }
        }
}

