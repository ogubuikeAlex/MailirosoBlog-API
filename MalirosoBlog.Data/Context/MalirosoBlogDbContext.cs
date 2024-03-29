

using MalirosoBlog.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MalirosoBlog.Data.Context
{
    public class MailRosoBlogDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public virtual DbSet<BlogPost> Blogs { get; set; }
        public virtual DbSet<Author> Authors { get; set; }        
        public virtual DbSet<ApplicationUserRole> UserRoles { get; set; }

        public MailRosoBlogDbContext(DbContextOptions<MailRosoBlogDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationUser>(b =>
            {
                b.Property(e => e.Id)
                    .ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<Author>(b =>
            {
                b.Property(e => e.Id)
                    .ValueGeneratedOnAdd();
            });
        }
    }
}