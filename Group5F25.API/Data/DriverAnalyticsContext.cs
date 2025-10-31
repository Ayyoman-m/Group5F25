using Microsoft.EntityFrameworkCore;
using Group5F25.API.Models;

namespace Group5F25.API.Data
{
    // This class connects the app to the database using Entity Framework Core
    public class DriverAnalyticsContext : DbContext
    {
        // Constructor passes options (like connection string) to the base DbContext
        public DriverAnalyticsContext(DbContextOptions<DriverAnalyticsContext> options) : base(options) { }

        // Table for storing user records in the database
        public DbSet<User> Users => Set<User>();

        // This method sets up special rules for the database
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Make sure each user has a unique email (no duplicates allowed)
            modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();

            base.OnModelCreating(modelBuilder);
        }
    }
}
