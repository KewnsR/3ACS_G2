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
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Department> Departments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
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
                    .HasColumnName("Username")
                    .HasMaxLength(50);

                entity.Property(u => u.Password)
                    .IsRequired()
                    .HasColumnName("Password")
                    .HasMaxLength(255);

                entity.Property(u => u.LastLogin)
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
            });

            // Configure Department (Departments table)
            modelBuilder.Entity<Department>(entity =>
            {
                entity.ToTable("Departments");
                entity.HasKey(d => d.DepartmentID);

                entity.Property(d => d.DepartmentID)
                    .HasColumnName("DepartmentID")
                    .ValueGeneratedOnAdd();

                // Match these to your actual database column names
                entity.Property(d => d.Name)
                    .IsRequired()
                    .HasColumnName("DepartmentName") // Updated to match DB
                    .HasMaxLength(100);

            
            });

            // Configure Employee (Employees table)
            modelBuilder.Entity<Employee>(entity =>
            {
                entity.ToTable("Employees");
                entity.HasKey(e => e.EmployeeID);

                entity.Property(e => e.EmployeeID)
                    .HasColumnName("EmployeeID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasColumnName("FirstName")
                    .HasMaxLength(100);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasColumnName("LastName")
                    .HasMaxLength(100);

                entity.Property(e => e.Email)
                    .HasColumnName("Email")
                    .HasMaxLength(255)
                    .IsRequired(false);

                entity.Property(e => e.PhoneNumber)
                    .HasColumnName("PhoneNumber")
                    .HasMaxLength(20)
                    .IsRequired(false);

                entity.Property(e => e.DepartmentID)
                    .IsRequired()
                    .HasColumnName("DepartmentID");

                entity.Property(e => e.Position)
                    .IsRequired()
                    .HasColumnName("Position")
                    .HasMaxLength(100);

                entity.Property(e => e.Salary)
                    .IsRequired()
                    .HasColumnName("Salary")
                    .HasColumnType("decimal(18,2)");

                entity.Property(e => e.DateHired)
                    .IsRequired()
                    .HasColumnName("DateHired")
                    .HasColumnType("date");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasColumnName("Status")
                    .HasMaxLength(50)
                    .HasDefaultValue("Active");

                // Relationship with Department
                entity.HasOne(e => e.Department)
                    .WithMany(d => d.Employees)
                    .HasForeignKey(e => e.DepartmentID)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Relationship between ApplicationUser and Employee
            modelBuilder.Entity<ApplicationUser>()
                .HasOne<Employee>()
                .WithMany()
                .HasForeignKey(u => u.EmployeeID)
                .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);
        }
    }
}