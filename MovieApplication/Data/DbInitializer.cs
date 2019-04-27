using Microsoft.AspNetCore.Identity;
using MovieApplication.Models;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace MovieApplication.Data
{
    public class DbInitializer
    {
        public static async Task Initialize(ApplicationDbContext context)
        {
            context.Database.EnsureCreated();
            // Look for any students.
            if (context.Movies.Any())
            {
                return;   // DB has been seeded
            }

            var movies = new Movie[]
            {
            new Movie{Title="Shrek", Year=2001},
            new Movie{Title="Avengers", Year=2008},
            new Movie{Title="Cars", Year=2006},
            new Movie{Title="Shrek 2", Year=2004},
            new Movie{Title="Test", Year=2019}
            };
            foreach (Movie m in movies)
            {
                context.Movies.Add(m);
            }
            context.SaveChanges();

            var categories = new Category[]
            {
            new Category{Name="Comedy"},
            new Category{Name="Action"},
            };
            foreach (Category c in categories)
            {
                context.Categories.Add(c);
            }
            context.SaveChanges();

            var movies_categories = new MovieCategory[]
            {
            new MovieCategory{MovieId=1, CategoryId=1},
            new MovieCategory{MovieId=2, CategoryId=1},
            new MovieCategory{MovieId=2, CategoryId=2},
            new MovieCategory{MovieId=3, CategoryId=1},
            new MovieCategory{MovieId=4, CategoryId=1},
            new MovieCategory{MovieId=5, CategoryId=2},
            };
            foreach (MovieCategory m in movies_categories)
            {
                context.MovieCategories.Add(m);
            }
            context.SaveChanges();

            //Create Admin User
           var userManager = context.GetService<UserManager<ApplicationUser>>();
           var AdminUser= new ApplicationUser { UserName = "jack@movies.com", Email = "jack@movies.com"};
           IdentityResult UserResult = userManager.CreateAsync(AdminUser, "Password123$").Result;

            //Create Admin Role
           var roleManager = context.GetService<RoleManager<IdentityRole>>();
           var indentityRole = new IdentityRole("Admin");
           IdentityResult RoleResult = roleManager.CreateAsync(indentityRole).Result;
            //Assign admin user to admin role
            if (RoleResult.Succeeded && UserResult.Succeeded)
            {
                userManager.AddToRoleAsync(AdminUser, "Admin").Wait();
            }

            

          

            //context.SaveChanges()

        }



    }

    

}
