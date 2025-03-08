using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KSPRAS.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AbstractSubmissionModel",
                columns: table => new
                {
                    ID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Institution = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PresentingAuthor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CoAuthors = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PresentationType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ConferenceType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Introduction = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Methods = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Results = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Conclusion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    isPaid = table.Column<bool>(type: "bit", nullable: false),
                    ammount = table.Column<float>(type: "real", nullable: false),
                    reffCode = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbstractSubmissionModel", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "IPNResponses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderTrackingId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrderNotificationType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrderMerchantReference = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IPNResponses", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AbstractSubmissionModel");

            migrationBuilder.DropTable(
                name: "IPNResponses");
        }
    }
}
