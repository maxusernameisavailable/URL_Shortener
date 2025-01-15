using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using URL_Shortener1.Models;

namespace URL_Shortener1.DBContext
{
    public class ApplicationDBContext : IdentityDbContext<User, Role, int>
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options) { }

        public DbSet<URL> URLs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<URL>()
                .HasOne(ur => ur.User)
                .WithMany(us => us.URLs)
                .HasForeignKey(ur => ur.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            //modelBuilder.Entity<URL>(url => url.Property(u => u.CreatedDate).HasColumnType("timestamp with time zone"));
        }

    }
}
