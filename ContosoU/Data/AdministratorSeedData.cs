using ContosoU.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContosoU.Data
{
    public class AdministratorSeedData
    {
        private readonly RoleManager<IdentityRole> _roleManeger;
        private readonly UserManager<ApplicationUser> _userManager;
        public AdministratorSeedData(RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> userManager)
        {
            _roleManeger = roleManager;
            _userManager = userManager;
        }

        public async Task EnsureSeedDatat()
        {
            var adminRole = await _roleManeger.FindByNameAsync("admin");
            if(adminRole == null)
            {
                adminRole = new IdentityRole("admin");
                await _roleManeger.CreateAsync(adminRole);
            }

            ApplicationUser adminUser = new ApplicationUser
            {
                UserName = "admin@contoso.com",
                Email = "admin@contoso.com"
            };

            await _userManager.CreateAsync(adminUser, "youradminpassword");
            await _userManager.SetLockoutEnabledAsync(adminUser, false);

            IdentityResult result = await _userManager.AddToRoleAsync(adminUser, "admin");
        }
    }
}
