using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Book.Archieve.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class NoteSharingFixed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShareId",
                table: "Note");

            migrationBuilder.AddColumn<int>(
                name: "ShareId",
                table: "User",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsShared",
                table: "Note",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShareId",
                table: "User");

            migrationBuilder.DropColumn(
                name: "IsShared",
                table: "Note");

            migrationBuilder.AddColumn<int>(
                name: "ShareId",
                table: "Note",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
