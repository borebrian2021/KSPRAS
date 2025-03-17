using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KSPRAS.Migrations
{
    /// <inheritdoc />
    public partial class V3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "reffCode",
                table: "Registrations",
                newName: "ReffCode");

            migrationBuilder.RenameColumn(
                name: "ammount",
                table: "Registrations",
                newName: "Ammount");

            migrationBuilder.AddColumn<string>(
                name: "Status_code",
                table: "Registrations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status_code",
                table: "Registrations");

            migrationBuilder.RenameColumn(
                name: "ReffCode",
                table: "Registrations",
                newName: "reffCode");

            migrationBuilder.RenameColumn(
                name: "Ammount",
                table: "Registrations",
                newName: "ammount");
        }
    }
}
