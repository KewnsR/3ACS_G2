using HumanRepProj.Models;
using Microsoft.EntityFrameworkCore;

namespace HumanRepProj.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApplicationUser>()
                .ToTable("Logins") // Match the table name in the database
                .HasKey(u => u.LoginID);

            // Map fields to columns explicitly to avoid mismatches
            modelBuilder.Entity<ApplicationUser>()
                .Property(u => u.LoginID).HasColumnName("LoginID");
            modelBuilder.Entity<ApplicationUser>()
                .Property(u => u.EmployeeID).HasColumnName("EmployeeID");
            modelBuilder.Entity<ApplicationUser>()
                .Property(u => u.Username).HasColumnName("Username");
            modelBuilder.Entity<ApplicationUser>()
                .Property(u => u.PasswordHash).HasColumnName("PasswordHash");
            modelBuilder.Entity<ApplicationUser>()
                .Property(u => u.LastLogin).HasColumnName("LastLogin");
            modelBuilder.Entity<ApplicationUser>()
                .Property(u => u.FailedAttempts).HasColumnName("FailedAttempts");
            modelBuilder.Entity<ApplicationUser>()
                .Property(u => u.IsLocked).HasColumnName("IsLocked");
            modelBuilder.Entity<ApplicationUser>()
                .Property(u => u.CreatedAt).HasColumnName("CreatedAt");
            modelBuilder.Entity<ApplicationUser>()
                .Property(u => u.UpdatedAt).HasColumnName("UpdatedAt");

            base.OnModelCreating(modelBuilder);
        }
    }
}
