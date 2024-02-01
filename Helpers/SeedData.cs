using AccessoriesWebsite.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace AccessoriesWebsite.Helpers
{
    public static class SeedData
    {


        public static void Seed(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var dbContext = serviceProvider.GetRequiredService<ApplicationDBContext>();


            //create Roles
            if (!roleManager.RoleExistsAsync("Admin").GetAwaiter().GetResult())
            {
                roleManager.CreateAsync(new IdentityRole("Admin")).GetAwaiter().GetResult();
                roleManager.CreateAsync(new IdentityRole("User")).GetAwaiter().GetResult();
            }

            //Create Admin
            userManager.CreateAsync(new ApplicationUser
            {
                UserName = "Ayaa",
                Email = "aya.ayman@gmail.com"
            }, "Aa123456#*@").GetAwaiter().GetResult();

            //check admin exist
            var AppAdmin = userManager.FindByEmailAsync("aya.ayman@gmail.com").GetAwaiter().GetResult();

            //asign role to admin
            if (AppAdmin != null)
            {
                userManager.AddToRoleAsync(AppAdmin, "Admin").GetAwaiter().GetResult();
            }
            //userManager.AddClaimAsync(AppAdmin, new Claim(ClaimTypes.Role, WebSiteRoles.SiteAdmin)).GetAwaiter().GetResult();


            dbContext.SaveChanges();




        }

    }
}
