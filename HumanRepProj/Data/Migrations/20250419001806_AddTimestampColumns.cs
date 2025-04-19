using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace HumanRepProj.Data.Migrations
{
    public partial class AddTimestampColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Add to Employees table
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Employees",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETDATE()");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Employees",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETDATE()");

            // Add to Departments table
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Departments",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETDATE()");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Departments",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETDATE()");

            // Add to Logins table
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Logins",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETDATE()");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Logins",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETDATE()");

            // Create trigger to auto-update UpdatedAt on Employees
            migrationBuilder.Sql(@"
                CREATE TRIGGER tr_Employees_UpdateTimestamp
                ON Employees
                AFTER UPDATE
                AS
                BEGIN
                    UPDATE Employees
                    SET UpdatedAt = GETDATE()
                    FROM Employees e
                    INNER JOIN inserted i ON e.EmployeeID = i.EmployeeID
                END
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Remove trigger first
            migrationBuilder.Sql("DROP TRIGGER IF EXISTS tr_Employees_UpdateTimestamp");

            // Remove from Employees
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Employees");

            // Remove from Departments
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Departments");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Departments");

            // Remove from Logins
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Logins");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Logins");
        }
    }
}