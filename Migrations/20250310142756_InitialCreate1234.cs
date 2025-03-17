using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KSPRAS.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate1234 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isPaid",
                table: "Registrations");

            migrationBuilder.AddColumn<string>(
                name: "Payment_Status",
                table: "Registrations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Payment_Status",
                table: "Registrations");

            migrationBuilder.AddColumn<bool>(
                name: "isPaid",
                table: "Registrations",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
