using MalirosoBlog.Data.Context;
using MalirosoBlog.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MalirosoBlog.API.DataSeeder
{
    public class Seed
    {
        public IEnumerable<string> Roles { get; set; }

        public AdminUser AdminUser { get; set; }
    }

    public class AdminUser
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Role { get; set; }
        public string Password { get; set; }
    }

    public static class SeedAppData
    {
        public static IServiceCollection BindSeedDataConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            Seed seed = new();

            configuration.GetSection("Seed").Bind(seed);

            services.AddSingleton(seed);

            return services;
        }

        public static async Task EnsurePopulated(IApplicationBuilder app)
        {
            Seed seed = app.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<Seed>();

            MailRosoBlogDbContext context = app.ApplicationServices
                .CreateScope().ServiceProvider.GetRequiredService<MailRosoBlogDbContext>();

            UserManager<ApplicationUser> userManager = app.ApplicationServices
                .CreateScope().ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            RoleManager<ApplicationRole> roleManager = app.ApplicationServices
                .CreateScope().ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();

            if (!await roleManager.Roles.AnyAsync())
            {
                foreach (string role in seed.Roles)
                {
                    await roleManager.CreateAsync(new ApplicationRole { Name = role });
                }
            }

            if (!await userManager.Users.AnyAsync())
            {
                ApplicationUser admin = new()
                {
                    FirstName = seed.AdminUser.FirstName,
                    LastName = seed.AdminUser.LastName,
                    Email = seed.AdminUser.Email,
                    Active = true,
                    EmailConfirmed = true,
                    UserName = seed.AdminUser.Email
                };

                ApplicationUser author = new()
                {
                    FirstName = "John",
                    LastName = "Doe",
                    Email = "johndoe@yopmail.com",
                    Active = true,
                    EmailConfirmed = true,
                    UserName = "johndoe@yopmail.com",
                };

                IdentityResult createAdmin = await userManager.CreateAsync(admin, seed.AdminUser.Password);

                IdentityResult createAuthor = await userManager.CreateAsync(author, seed.AdminUser.Password);

                if (createAdmin.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, seed.AdminUser.Role);
                }

                if (createAuthor.Succeeded)
                {
                    Author authorProfile = new() { UserId = author.Id };

                    context.Authors.Add(authorProfile);

                    await userManager.AddToRoleAsync(author, "Author");

                    context.Blogs.AddRange(
                        [
                            new() { Title = "Brinky: The Biscuit of Biscuits", Content = "Colombina Have Truly Outdone Themselves Here!", AuthorId = authorProfile.Id },
                            new() { Title = "Food: A Blessing To Humans", Content = "Really, We All Need To Eat More", AuthorId = authorProfile.Id },
                            new() { Title = "Eating: The Habit We Must Make Queen", Content = "Asides Praying and Eating, What Other Habit Is Worthy?", AuthorId = authorProfile.Id }
                        ]);

                    await context.SaveChangesAsync();
                }
            }
        }
    }
}
