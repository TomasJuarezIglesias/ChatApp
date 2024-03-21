using ChatApp_API.Models;
using Microsoft.EntityFrameworkCore;

namespace ChatApp_API
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasIndex(u => new { u.UserName, u.Email })
                .IsUnique();

            modelBuilder.Entity<Message>()
            .HasOne(m => m.UserSender)
            .WithMany(u => u.SentMessages)
            .HasForeignKey(m => m.UserSenderId)
            .OnDelete(DeleteBehavior.Restrict);
            
            modelBuilder.Entity<Message>()
            .HasOne(m => m.UserReceive)
            .WithMany(u => u.ReceivedMessages)
            .HasForeignKey(m => m.UserReceiveId)
            .OnDelete(DeleteBehavior.Restrict);
        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Message> Messages { get; set; }

    }
}
