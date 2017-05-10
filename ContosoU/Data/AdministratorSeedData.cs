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
            //check if we have existing admin user(admin@contoso.com) do not want to rerun this part
            if (await _userManager.FindByEmailAsync("admin@contoso.com") == null)
            {
                var adminRole = await _roleManeger.FindByNameAsync("admin");
                if (adminRole == null)
                {
                    //admin role does not exist - create it
                    adminRole = new IdentityRole("admin");
                    await _roleManeger.CreateAsync(adminRole);
                }

                //create the 'admin' user
                ApplicationUser adminUser = new ApplicationUser
                {
                    UserName = "admin@contoso.com",
                    Email = "admin@contoso.com"
                };

                await _userManager.CreateAsync(adminUser, "Admin@123456");
                await _userManager.SetLockoutEnabledAsync(adminUser, false);

                IdentityResult result = await _userManager.AddToRoleAsync(adminUser, "admin");
            }

            //create 'student' role
            var studentRole = await _roleManeger.FindByNameAsync("student");
            if(studentRole == null)
            {
                //role did not exist -create it
                studentRole = new IdentityRole("student");
                await _roleManeger.CreateAsync(studentRole);
            }

            //create 'instructor' role
            var instructorRole = await _roleManeger.FindByNameAsync("instructor");
            if (studentRole == null)
            {
                //role did not exist -create it
                instructorRole = new IdentityRole("instructor");
                await _roleManeger.CreateAsync(instructorRole);
            }
        }
    }
}
