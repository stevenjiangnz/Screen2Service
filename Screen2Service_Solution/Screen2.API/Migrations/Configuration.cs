//namespace Screen2.Api.Migrations
//{
//    using DAL;
//    using Infrastructure;
//    using Microsoft.AspNet.Identity;
//    using Microsoft.AspNet.Identity.EntityFramework;
//    using System;
//    using System.Data.Entity;
//    using System.Data.Entity.Migrations;
//    using System.Linq;

//    internal sealed class Configuration : DbMigrationsConfiguration<Screen2.Api.Infrastructure.ApplicationDbContext>
//    {
//        public Configuration()
//        {
//            AutomaticMigrationsEnabled = false;
//        }

//        protected override void Seed(Screen2.Api.Infrastructure.ApplicationDbContext context)
//        {
//            //  This method will be called after migrating to the latest version.

//            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));

//            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));

//            var user = new ApplicationUser()
//            {
//                UserName = "stevenjiangnz",
//                Email = "stevenjiangnz@gmail.com",
//                EmailConfirmed = true,
//                FirstName = "Steven",
//                LastName = "Jiang",
//                Level = 1,
//                JoinDate = DateTime.Now.AddYears(-3)
//            };

//            manager.Create(user, "L@ve@ver77");

//            if (roleManager.Roles.Count() == 0)
//            {
//                roleManager.Create(new IdentityRole { Name = "SuperAdmin" });
//                roleManager.Create(new IdentityRole { Name = "Admin" });
//                roleManager.Create(new IdentityRole { Name = "User" });
//            }

//            var adminUser = manager.FindByName("stevenjiangnz");

//            manager.AddToRoles(adminUser.Id, new string[] { "SuperAdmin", "Admin" });
//        }
//    }
//}
