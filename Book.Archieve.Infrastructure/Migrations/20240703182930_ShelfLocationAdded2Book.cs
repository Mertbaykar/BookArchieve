using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Book.Archieve.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ShelfLocationAdded2Book : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ShelfLocation",
                table: "Book",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShelfLocation",
                table: "Book");
        }
    }
}
