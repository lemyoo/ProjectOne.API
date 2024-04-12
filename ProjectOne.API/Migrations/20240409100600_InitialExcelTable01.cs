using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectOne.API.Migrations
{
    /// <inheritdoc />
    public partial class InitialExcelTable01 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Tickets",
                newName: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Tickets",
                newName: "ID");
        }
    }
}
