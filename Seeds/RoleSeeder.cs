using BookStoresWebAPI.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;

namespace BookStoresWebAPI.Seeds
{
    public class RoleSeeder
    {
        public static async Task SeedRoleAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<AppRole>>();

            string[] roles = { "Admin" ,"User", "Viewer" }; //roller aspnet roles tablosunda saklanır

            foreach (var roleName in roles)
            {
                if(!await roleManager.RoleExistsAsync(roleName))
                {
                    var role = new AppRole { Name = roleName, RoleDesc = $"{roleName} role" };
                    await roleManager.CreateAsync(role);
                    {
                                          
                    }
                }
            }
        }


    }
}
