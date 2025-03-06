﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KSPRAS.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AbstractSubmissionModel");
        }
    }
}
