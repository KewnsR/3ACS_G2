using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace HumanRepProj.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20250419001617_FixedRelationships")]
    public partial class FixedRelationships : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 1. First remove the foreign key constraint if it exists
            migrationBuilder.Sql(@"
                IF EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Employees_Employees_EmployeeID1')
                BEGIN
                    ALTER TABLE [dbo].[Employees] DROP CONSTRAINT [FK_Employees_Employees_EmployeeID1];
                END
            ");

            // 2. Remove the index if it exists
            migrationBuilder.Sql(@"
                IF EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Employees_EmployeeID1' AND object_id = OBJECT_ID('[dbo].[Employees]'))
                BEGIN
                    DROP INDEX [IX_Employees_EmployeeID1] ON [dbo].[Employees];
                END
            ");

            // 3. Finally remove the column if it exists
            migrationBuilder.Sql(@"
                IF EXISTS (SELECT * FROM sys.columns WHERE name = 'EmployeeID1' AND object_id = OBJECT_ID('[dbo].[Employees]'))
                BEGIN
                    ALTER TABLE [dbo].[Employees] DROP COLUMN [EmployeeID1];
                END
            ");

            // 4. Ensure the proper Manager relationship is maintained
            migrationBuilder.CreateIndex(
                name: "IX_Employees_ManagerID",
                table: "Employees",
                column: "ManagerID");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Employees_ManagerID",
                table: "Employees",
                column: "ManagerID",
                principalTable: "Employees",
                principalColumn: "EmployeeID",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // 1. Remove the proper foreign key we added
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Employees_ManagerID",
                table: "Employees");

            // 2. Recreate the EmployeeID1 column
            migrationBuilder.AddColumn<int>(
                name: "EmployeeID1",
                table: "Employees",
                type: "int",
                nullable: true);

            // 3. Recreate the index
            migrationBuilder.CreateIndex(
                name: "IX_Employees_EmployeeID1",
                table: "Employees",
                column: "EmployeeID1");

            // 4. Recreate the foreign key
            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Employees_EmployeeID1",
                table: "Employees",
                column: "EmployeeID1",
                principalTable: "Employees",
                principalColumn: "EmployeeID",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("HumanRepProj.Models.ApplicationUser", b =>
            {
                b.Property<int>("LoginID")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("int")
                    .HasColumnName("LoginID");

                SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("LoginID"));

                b.Property<DateTime>("CreatedAt")
                    .HasColumnType("datetime2");

                b.Property<int>("EmployeeID")
                    .HasColumnType("int")
                    .HasColumnName("EmployeeID");

                b.Property<int>("FailedAttempts")
                    .HasColumnType("int");

                b.Property<bool>("IsLocked")
                    .HasColumnType("bit");

                b.Property<DateTime?>("LastLogin")
                    .HasColumnType("datetime2")
                    .HasColumnName("LastLogin");

                b.Property<string>("Password")
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnType("nvarchar(255)")
                    .HasColumnName("Password");

                b.Property<DateTime>("UpdatedAt")
                    .HasColumnType("datetime2");

                b.Property<string>("Username")
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnType("nvarchar(50)")
                    .HasColumnName("Username");

                b.HasKey("LoginID");

                b.HasIndex("EmployeeID");

                b.HasIndex("Username")
                    .IsUnique()
                    .HasDatabaseName("IX_Logins_Username");

                b.ToTable("Logins", (string)null);
            });

            modelBuilder.Entity("HumanRepProj.Models.Department", b =>
            {
                b.Property<int>("DepartmentID")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("int")
                    .HasColumnName("DepartmentID");

                SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("DepartmentID"));

                b.Property<DateTime>("CreatedAt")
                    .HasColumnType("datetime2");

                b.Property<string>("Name")
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnType("nvarchar(100)")
                    .HasColumnName("DepartmentName");

                b.Property<DateTime>("UpdatedAt")
                    .HasColumnType("datetime2");

                b.HasKey("DepartmentID");

                b.HasIndex("Name")
                    .IsUnique()
                    .HasDatabaseName("IX_Departments_Name");

                b.ToTable("Departments", (string)null);
            });

            modelBuilder.Entity("HumanRepProj.Models.Employee", b =>
            {
                b.Property<int>("EmployeeID")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("int")
                    .HasColumnName("EmployeeID");

                SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("EmployeeID"));

                b.Property<string>("Address")
                    .HasMaxLength(500)
                    .HasColumnType("nvarchar(500)")
                    .HasColumnName("Address");

                b.Property<DateTime>("CreatedAt")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("datetime2")
                    .HasColumnName("CreatedAt")
                    .HasDefaultValueSql("GETDATE()");

                b.Property<DateTime>("DateHired")
                    .HasColumnType("date")
                    .HasColumnName("DateHired");

                b.Property<DateTime>("DateOfBirth")
                    .HasColumnType("date")
                    .HasColumnName("DateOfBirth");

                b.Property<int>("DepartmentID")
                    .HasColumnType("int")
                    .HasColumnName("DepartmentID");

                b.Property<string>("Email")
                    .HasMaxLength(255)
                    .HasColumnType("nvarchar(255)")
                    .HasColumnName("Email");

                b.Property<string>("EmploymentType")
                    .IsRequired()
                    .ValueGeneratedOnAdd()
                    .HasMaxLength(20)
                    .HasColumnType("nvarchar(20)")
                    .HasDefaultValue("Full-time")
                    .HasColumnName("EmploymentType");

                b.Property<string>("FirstName")
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnType("nvarchar(100)")
                    .HasColumnName("FirstName");

                b.Property<string>("Gender")
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasColumnType("nvarchar(10)");

                b.Property<bool>("IsManager")
                    .HasColumnType("bit");

                b.Property<string>("LastName")
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnType("nvarchar(100)")
                    .HasColumnName("LastName");

                b.Property<int?>("ManagerID")
                    .HasColumnType("int")
                    .HasColumnName("ManagerID");

                b.Property<string>("PhoneNumber")
                    .HasMaxLength(20)
                    .HasColumnType("nvarchar(20)")
                    .HasColumnName("PhoneNumber");

                b.Property<string>("Position")
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnType("nvarchar(100)")
                    .HasColumnName("Position");

                b.Property<decimal>("Salary")
                    .HasColumnType("decimal(18,2)")
                    .HasColumnName("Salary");

                b.Property<string>("Status")
                    .IsRequired()
                    .ValueGeneratedOnAdd()
                    .HasMaxLength(50)
                    .HasColumnType("nvarchar(50)")
                    .HasDefaultValue("Active")
                    .HasColumnName("Status");

                b.Property<DateTime>("UpdatedAt")
                    .ValueGeneratedOnAddOrUpdate()
                    .HasColumnType("datetime2")
                    .HasColumnName("UpdatedAt")
                    .HasDefaultValueSql("GETDATE()");

                b.HasKey("EmployeeID");

                b.HasIndex("DepartmentID");

                b.HasIndex("Email")
                    .IsUnique()
                    .HasDatabaseName("IX_Employees_Email")
                    .HasFilter("[Email] IS NOT NULL");

                b.HasIndex("ManagerID");

                b.ToTable("Employees", (string)null);
            });

            modelBuilder.Entity("HumanRepProj.Models.ApplicationUser", b =>
            {
                b.HasOne("HumanRepProj.Models.Employee", "Employee")
                    .WithMany()
                    .HasForeignKey("EmployeeID")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.Navigation("Employee");
            });

            modelBuilder.Entity("HumanRepProj.Models.Employee", b =>
            {
                b.HasOne("HumanRepProj.Models.Department", "Department")
                    .WithMany("Employees")
                    .HasForeignKey("DepartmentID")
                    .OnDelete(DeleteBehavior.Restrict)
                    .IsRequired();

                b.HasOne("HumanRepProj.Models.Employee", "Manager")
                    .WithMany("Subordinates")
                    .HasForeignKey("ManagerID")
                    .OnDelete(DeleteBehavior.NoAction);

                b.Navigation("Department");

                b.Navigation("Manager");
            });

            modelBuilder.Entity("HumanRepProj.Models.Department", b =>
            {
                b.Navigation("Employees");
            });

            modelBuilder.Entity("HumanRepProj.Models.Employee", b =>
            {
                b.Navigation("Subordinates");
            });
#pragma warning restore 612, 618
        }
    }
}