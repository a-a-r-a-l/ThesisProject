using Microsoft.AspNetCore.Identity;
using MyUniversity.Models;
using MyUniversity.Models.Enum;
using System;
using System.Threading.Tasks;

namespace MyUniversity.Data
{
    public static class ApplicationDbContextSeed
    {

        public static async Task SeedIdentityRolesAsync(RoleManager<MyIdentityRole> rolefaculty)
        {
            foreach (MyIdentityRoleNames role in Enum.GetValues(typeof(MyIdentityRoleNames)))
            {
                string rolename = role.ToString();
                if (!await rolefaculty.RoleExistsAsync(rolename))
                {
                    await rolefaculty.CreateAsync(
                        new MyIdentityRole { Name = rolename });
                }
            }
        }


        public static async Task SeedIdentityUserAsync(UserManager<MyIdentityUser> userfaculty)
        {
            MyIdentityUser user;

            user = await userfaculty.FindByNameAsync("admin@myuni.com");
            if (user == null)
            {
                user = new MyIdentityUser()
                {
                    UserName = "admin@myuni.com",
                    Email = "admin@myuni.com",
                    EmailConfirmed = true,
                    DisplayName = "The Admin User",
                    DateOfBirth = new DateTime(2000, 1, 1),
                    Gender = MyIdentityGenders.Female,
                    IsAdminUser = true

                };
                await userfaculty.CreateAsync(user, password: "Password@123");
                await userfaculty.AddToRolesAsync(user, new string[] {
                    MyIdentityRoleNames.Administrator.ToString(),
                    MyIdentityRoleNames.Faculty.ToString()
                });
            }

            user = await userfaculty.FindByNameAsync("faculty@myuni.com");
            if (user == null)
            {
                user = new MyIdentityUser()
                {
                    UserName = "faculty@myuni.com",
                    Email = "faculty@myuni.com",
                    EmailConfirmed = true,
                    DisplayName = "The Faculty",
                    DateOfBirth = new DateTime(2000, 1, 1),
                    Gender = MyIdentityGenders.Male,
                    IsAdminUser = true

                };
                await userfaculty.CreateAsync(user, password: "Asdf@123");
                await userfaculty.AddToRolesAsync(user, new string[] {
                    MyIdentityRoleNames.Faculty.ToString()
                });
            }

        }
    }
}
