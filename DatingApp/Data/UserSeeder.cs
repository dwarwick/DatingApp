using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace DatingApp.Data
{
    public static class UserSeeder
    {
        public static async Task SeedUsersAndRolesAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            string[] roleNames = { "Admin", "User" };
            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new ApplicationRole { Name = roleName });
                }
            }

            // Seed Admin user
            var adminEmail = "admin@user.com";
            var adminPassword = "Admin123!";
            var adminConcurrencyStamp = "11111111-1111-1111-1111-111111111111";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true,
                    ConcurrencyStamp = adminConcurrencyStamp
                };
                await userManager.CreateAsync(adminUser, adminPassword);
            }
            else
            {
                if (adminUser.ConcurrencyStamp != adminConcurrencyStamp)
                {
                    adminUser.ConcurrencyStamp = adminConcurrencyStamp;
                    await userManager.UpdateAsync(adminUser);
                }
                if (!adminUser.EmailConfirmed)
                {
                    adminUser.EmailConfirmed = true;
                    await userManager.UpdateAsync(adminUser);
                }
            }
            if (!await userManager.IsInRoleAsync(adminUser, "Admin"))
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }

            // Seed regular user
            var userEmail = "user@user.com";
            var userPassword = "User123!";
            var userConcurrencyStamp = "22222222-2222-2222-2222-222222222222";
            var normalUser = await userManager.FindByEmailAsync(userEmail);
            if (normalUser == null)
            {
                normalUser = new ApplicationUser
                {
                    UserName = userEmail,
                    Email = userEmail,
                    EmailConfirmed = true,
                    ConcurrencyStamp = userConcurrencyStamp
                };
                await userManager.CreateAsync(normalUser, userPassword);
            }
            else
            {
                if (normalUser.ConcurrencyStamp != userConcurrencyStamp)
                {
                    normalUser.ConcurrencyStamp = userConcurrencyStamp;
                    await userManager.UpdateAsync(normalUser);
                }
                if (!normalUser.EmailConfirmed)
                {
                    normalUser.EmailConfirmed = true;
                    await userManager.UpdateAsync(normalUser);
                }
            }
            if (!await userManager.IsInRoleAsync(normalUser, "User"))
            {
                await userManager.AddToRoleAsync(normalUser, "User");
            }
        }
    }
}
