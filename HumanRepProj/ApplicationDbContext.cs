using HumanRepProj.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HumanRepProj.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Department> Departments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure ApplicationUser (Logins table)
            modelBuilder.Entity<ApplicationUser>(entity =>
            {
                entity.ToTable("Logins");
                entity.HasKey(u => u.LoginID);

                entity.Property(u => u.LoginID)
                    .HasColumnName("LoginID")
                    .ValueGeneratedOnAdd();

                entity.Property(u => u.EmployeeID)
                    .HasColumnName("EmployeeID");

                entity.Property(u => u.Username)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("Username");

                entity.Property(u => u.Password)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("Password");

                entity.Property(u => u.LastLogin)
                    .HasColumnType("datetime2")
                    .HasColumnName("LastLogin");

                entity.Property(u => u.FailedAttempts)
                    .HasColumnName("FailedAttempts")
                    .HasDefaultValue(0);

                entity.Property(u => u.IsLocked)
                    .HasColumnName("IsLocked")
                    .HasDefaultValue(false);

                entity.Property(u => u.CreatedAt)
                    .HasColumnName("CreatedAt")
                    .HasDefaultValueSql("GETDATE()");

                entity.Property(u => u.UpdatedAt)
                    .HasColumnName("UpdatedAt")
                    .HasDefaultValueSql("GETDATE()");

                entity.HasIndex(u => u.Username)
                    .IsUnique()
                    .HasDatabaseName("IX_Logins_Username");

                entity.HasOne(u => u.Employee)
                    .WithMany()
                    .HasForeignKey(u => u.EmployeeID)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure Department (Departments table)
            modelBuilder.Entity<Department>(entity =>
            {
                entity.ToTable("Departments");
                entity.HasKey(d => d.DepartmentID);

                entity.Property(d => d.DepartmentID)
                    .HasColumnName("DepartmentID")
                    .ValueGeneratedOnAdd();

                entity.Property(d => d.Name)
                    .IsRequired()
                    .HasColumnName("DepartmentName")
                    .HasMaxLength(100);

                entity.Property(d => d.Description)
                    .HasMaxLength(500);

                entity.Property(d => d.Performance)
                    .HasColumnType("decimal(18,2)");

                entity.Property(d => d.DateCreated)
                    .HasColumnType("datetime2")
                    .HasDefaultValueSql("GETDATE()");

                entity.Property(d => d.Budget)
                    .HasColumnType("decimal(18,2)");

                entity.Property(d => d.Status)
                    .HasMaxLength(50)
                    .HasDefaultValue("Active");

                entity.HasIndex(d => d.Name)
                    .IsUnique()
                    .HasDatabaseName("IX_Departments_Name");

                entity.HasMany(d => d.Employees)
                    .WithOne(e => e.Department)
                    .HasForeignKey(e => e.DepartmentID)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Configure Employee (Employees table)
            modelBuilder.Entity<Employee>(entity =>
            {
                entity.ToTable("Employees");
                entity.HasKey(e => e.EmployeeID);

                // Basic Info
                entity.Property(e => e.EmployeeID)
                    .HasColumnName("EmployeeID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("FirstName");

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("LastName");

                // Contact Info
                entity.Property(e => e.Email)
                    .HasMaxLength(255)
                    .HasColumnName("Email")
                    .IsRequired(false);

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(20)
                    .HasColumnName("PhoneNumber")
                    .IsRequired(false);

                entity.Property(e => e.Address)
                    .HasMaxLength(500)
                    .HasColumnName("Address")
                    .IsRequired(false);

                // Employment Details
                entity.Property(e => e.DepartmentID)
                    .IsRequired()
                    .HasColumnName("DepartmentID");

                entity.Property(e => e.Position)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("Position");

                entity.Property(e => e.Salary)
                    .IsRequired()
                    .HasColumnType("decimal(18,2)")
                    .HasColumnName("Salary");

                // Dates
                entity.Property(e => e.DateOfBirth)
                    .HasColumnType("date")
                    .HasColumnName("DateOfBirth");

                entity.Property(e => e.DateHired)
                    .IsRequired()
                    .HasColumnType("date")
                    .HasColumnName("DateHired");

                // Status
                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasDefaultValue("Active")
                    .HasColumnName("Status");

                entity.Property(e => e.EmploymentType)
                    .HasMaxLength(20)
                    .HasColumnName("EmploymentType")
                    .HasDefaultValue("Full-time");

                // Manager relationship
                entity.Property(e => e.ManagerID)
                    .HasColumnName("ManagerID")
                    .IsRequired(false);

                // Timestamps
                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime2")
                    .HasColumnName("CreatedAt")
                    .HasDefaultValueSql("GETDATE()");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime2")
                    .HasColumnName("UpdatedAt")
                    .ValueGeneratedOnAddOrUpdate()
                    .HasDefaultValueSql("GETDATE()");

                // Relationships
                entity.HasOne(e => e.Manager)
                    .WithMany(e => e.Subordinates)
                    .HasForeignKey(e => e.ManagerID)
                    .IsRequired(false)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(e => e.Department)
                    .WithMany(d => d.Employees)
                    .HasForeignKey(e => e.DepartmentID)
                    .OnDelete(DeleteBehavior.Restrict);

                // Indexes
                entity.HasIndex(e => e.Email)
                    .IsUnique()
                    .HasDatabaseName("IX_Employees_Email")
                    .HasFilter("[Email] IS NOT NULL");

                entity.HasIndex(e => e.ManagerID)
                    .HasDatabaseName("IX_Employees_ManagerID");
            });
        }

        public override int SaveChanges()
        {
            UpdateTimestamps();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateTimestamps();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void UpdateTimestamps()
        {
            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is BaseEntity &&
                          (e.State == EntityState.Added || e.State == EntityState.Modified));

            var currentTime = DateTime.UtcNow;

            foreach (var entry in entries)
            {
                var entity = (BaseEntity)entry.Entity;
                if (entry.State == EntityState.Added)
                {
                    entity.CreatedAt = currentTime;
                }
                entity.UpdatedAt = currentTime;
            }
        }
    }

    public abstract class BaseEntity
    {
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}