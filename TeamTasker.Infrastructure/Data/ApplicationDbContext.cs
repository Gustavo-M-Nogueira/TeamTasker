using Microsoft.EntityFrameworkCore;
using TeamTasker.Domain.Models;

namespace TeamTasker.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Domain.Models.Task> Tasks { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<User> Users { get; set; }

        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
            
        }
    }
}
