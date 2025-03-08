using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KSPRAS.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Key",
                table: "IPNResponses",
                newName: "Id");

            migrationBuilder.AlterColumn<string>(
                name: "OrderMerchantReference",
                table: "IPNResponses",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "IPNResponses",
                newName: "Key");

            migrationBuilder.AlterColumn<decimal>(
                name: "OrderMerchantReference",
                table: "IPNResponses",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
