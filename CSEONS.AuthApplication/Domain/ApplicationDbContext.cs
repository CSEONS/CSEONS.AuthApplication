using CSEONS.AuthApplication.Domain.Entities;
using CSEONS.AuthApplication.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CSEONS.AuthApplication.Domain
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Group> Groups { get; set; }
        public DbSet<Message> Messages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var admin = new ApplicationUser()
            {
                Id = "b12858e6-15b5-462a-aab8-bcdc557138d6",
                UserName = "admin",
                Login = "0000",
                FirstName = "Admin",
                LastName = "Admin",
                SecondName = "Admin",
                NormalizedUserName = "ADMIN",
                Email = "my@email.com",
                NormalizedEmail = "MY@EMAIL.COM",
                EmailConfirmed = true,
                PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(new ApplicationUser(), "superpassword"),
                SecurityStamp = string.Empty,
                
            };

            var adminGroup = new Group()
            {
                Id = "f406242b-c133-41a7-be9e-01a13625f8a8",
                Name = "AdminGroup"
            };

            var generalChat = new Group()
            {
                Id = "2b07c0ca-f203-4690-afb7-7ac3ab5e92c5",
                Name = "GeneralGroup"
            };

            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole()
                {
                    Id = new Guid("c7552534-a1b0-4afc-9763-0efa80f032c8").ToString(),
                    Name = nameof(ApplicationUser.Roles.Admin),
                    NormalizedName = nameof(ApplicationUser.Roles.Admin).ToUpper(),
                }, 
                new IdentityRole()
                {
                    Id = new Guid("873460da-9b94-4874-ba24-22daeb85a409").ToString(),
                    Name = nameof(ApplicationUser.Roles.Teacher),
                    NormalizedName = nameof(ApplicationUser.Roles.Teacher).ToUpper(),
                },
                new IdentityRole()
                {
                    Id = new Guid("3052ca6a-aab4-437f-acf4-2eb46b3eaccf").ToString(),
                    Name = nameof(ApplicationUser.Roles.Student),
                    NormalizedName = nameof(ApplicationUser.Roles.Student).ToUpper()
                });

            modelBuilder.Entity<Group>().HasData(adminGroup, generalChat);

            modelBuilder.Entity<ApplicationUser>().HasData(admin);

            modelBuilder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>()
            {
                RoleId = new Guid("c7552534-a1b0-4afc-9763-0efa80f032c8").ToString(),
                UserId = new Guid("b12858e6-15b5-462a-aab8-bcdc557138d6").ToString()
            });

            admin.GroupId = adminGroup.Id;

            base.OnModelCreating(modelBuilder);
        }
    }
}
