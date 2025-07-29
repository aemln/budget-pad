using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BudgetPad.Migrations
{
    /// <inheritdoc />
    public partial class ChangeBudgetDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Date",
                table: "Budgets",
                newName: "CreatedDate");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreatedDate",
                table: "Budgets",
                newName: "Date");
        }
    }
}
