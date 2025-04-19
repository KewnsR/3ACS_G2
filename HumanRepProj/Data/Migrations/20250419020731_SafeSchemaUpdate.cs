using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HumanRepProj.Data.Migrations
{
    /// <inheritdoc />
    public partial class SafeSchemaUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 1. First check if tables exist in wrong schema
            migrationBuilder.Sql(@"
        IF EXISTS (SELECT * FROM sys.tables WHERE name = 'Employees' AND schema_id != SCHEMA_ID('dbo'))
        BEGIN
            ALTER SCHEMA dbo TRANSFER [your_current_schema].[Employees];
        END
        
        IF EXISTS (SELECT * FROM sys.tables WHERE name = 'Logins' AND schema_id != SCHEMA_ID('dbo'))
        BEGIN
            ALTER SCHEMA dbo TRANSFER [your_current_schema].[Logins];
        END
    ");

            // 2. Rename table if it exists (instead of assuming)
            migrationBuilder.Sql(@"
        IF EXISTS (SELECT * FROM sys.tables WHERE name = 'Logins' AND schema_id = SCHEMA_ID('dbo'))
        BEGIN
            EXEC sp_rename 'dbo.Logins', 'ApplicationUsers';
        END
    ");

            // 3. Modify columns safely
            migrationBuilder.AlterColumn<string>(
                name: "Status",
                schema: "dbo",
                table: "Employees",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "Active");

            // 4. Recreate indexes only if they don't exist
            migrationBuilder.Sql(@"
        IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_ApplicationUsers_EmployeeID')
        BEGIN
            CREATE INDEX [IX_ApplicationUsers_EmployeeID] ON [dbo].[ApplicationUsers] ([EmployeeID]);
        END
    ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Reverse operations
            migrationBuilder.Sql(@"
        IF EXISTS (SELECT * FROM sys.tables WHERE name = 'ApplicationUsers')
        BEGIN
            EXEC sp_rename 'dbo.ApplicationUsers', 'Logins';
        END
    ");

            // Restore original column definitions
            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Employees",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "Active");
        }
    }
}
