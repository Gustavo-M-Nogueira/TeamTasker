using Microsoft.EntityFrameworkCore;
using TeamTasker.API.Models;

namespace TeamTasker.API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<API.Models.Task> Tasks { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<User> Users { get; set; }

        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasMany(e => e.Tasks)
                .WithMany(e => e.Users)
                .UsingEntity(
                    "UserTask",
                    l => l.HasOne(typeof(API.Models.Task))
                            .WithMany()
                            .HasForeignKey("UserID")
                            .HasPrincipalKey(nameof(User.Id)),
                    r => r.HasOne(typeof(User))
                            .WithMany()
                            .HasForeignKey("TaskID")
                            .HasPrincipalKey(nameof(API.Models.Task.Id)),
                    j => j.HasKey("UserID", "TaskID")
                );

            base.OnModelCreating(modelBuilder);
        }
    }
}
