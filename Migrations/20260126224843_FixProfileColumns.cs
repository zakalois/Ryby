using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ryby.Migrations
{
    public partial class FixProfileColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Smazat starý sloupec
            migrationBuilder.DropColumn(
                name: "ProfileImagePath",
                table: "AspNetUsers");

            // Přidat nový správný sloupec
            migrationBuilder.AddColumn<string>(
                name: "ProfilePicturePath",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Vrátit zpět
            migrationBuilder.DropColumn(
                name: "ProfilePicturePath",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "ProfileImagePath",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}