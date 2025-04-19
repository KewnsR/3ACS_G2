using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HumanRepProj.Data.Migrations
{
    public partial class RemoveEmployeeID1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Safely remove the foreign key if it exists
            migrationBuilder.Sql(@"
                IF EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Employees_Employees_EmployeeID1')
                ALTER TABLE [Employees] DROP CONSTRAINT [FK_Employees_Employees_EmployeeID1];
            ");

            // Safely remove the index if it exists
            migrationBuilder.Sql(@"
                IF EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Employees_EmployeeID1' AND object_id = OBJECT_ID('Employees'))
                DROP INDEX [IX_Employees_EmployeeID1] ON [Employees];
            ");

            // Safely remove the column if it exists
            migrationBuilder.Sql(@"
                IF EXISTS (SELECT * FROM sys.columns WHERE name = 'EmployeeID1' AND object_id = OBJECT_ID('Employees'))
                ALTER TABLE [Employees] DROP COLUMN [EmployeeID1];
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Recreate the column (nullable)
            migrationBuilder.AddColumn<int>(
                name: "EmployeeID1",
                table: "Employees",
                nullable: true);

            // Recreate the index
            migrationBuilder.CreateIndex(
                name: "IX_Employees_EmployeeID1",
                table: "Employees",
                column: "EmployeeID1");

            // Recreate the foreign key
            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Employees_EmployeeID1",
                table: "Employees",
                column: "EmployeeID1",
                principalTable: "Employees",
                principalColumn: "EmployeeID",
                onDelete: ReferentialAction.NoAction);
        }
    }
}