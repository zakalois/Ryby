using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ryby.Migrations
{
    /// <inheritdoc />
    public partial class AddProfilePictureColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Phone",
                table: "AspNetUsers",
                newName: "ProfilePictureLargePath");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProfilePictureLargePath",
                table: "AspNetUsers",
                newName: "Phone");
        }
    }
}
