using ChatApp_API.Models;
using Microsoft.EntityFrameworkCore;

namespace ChatApp_API
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasIndex(u => new { u.UserName, u.Email })
                .IsUnique();
        }

        public virtual DbSet<User> Users { get; set; }

    }
}
