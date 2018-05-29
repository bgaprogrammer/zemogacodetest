using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using JR.FinancialManager.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace JR.FinancialManager.Core.Data
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new FinancialManagerDbContext(serviceProvider.GetRequiredService<DbContextOptions<FinancialManagerDbContext>>()))
            {
                const string defaultPwd = "1234";

                //Assistant Role
                const string assistantRole = "Assistant";
                const string assistantUsername = "assistant@jrzemogacodetest.com";
                var assistantId = await CreateUser(serviceProvider, defaultPwd, assistantUsername, assistantRole);

                //Manager Role
                const string managerRole = "Manager";
                const string managerUsername = "manager@jrzemogacodetest.com";
                var managerId = await CreateUser(serviceProvider, defaultPwd, managerUsername, managerRole);

                //Administrator Role
                const string administratorRole = "Administrator";
                const string administratorUsername = "administrator@jrzemogacodetest.com";
                var administratorId = await CreateUser(serviceProvider, defaultPwd, administratorUsername, administratorRole);

                SeedDb(context);
            }
        }

        private static async Task<string> CreateUser(IServiceProvider serviceProvider, string testUserPw, string UserName, string role)
        {
            var userManager = serviceProvider.GetService<UserManager<ApplicationUser>>();

            var user = await userManager.FindByNameAsync(UserName);

            if (user == null)
            {
                user = new ApplicationUser { UserName = UserName, Email = UserName };
                await userManager.CreateAsync(user, testUserPw);
            }

            var roleManager = serviceProvider.GetService<RoleManager<IdentityRole>>();

            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }

            await userManager.AddToRoleAsync(user, role);

            return user.Id;
        }

        private static void SeedDb(FinancialManagerDbContext context)
        {
            //If default data is needed must be added here
            //For this example default transactions were added according to the dump .csv file provided by Zemoga

            //context.SaveChanges();
        }
    }
}