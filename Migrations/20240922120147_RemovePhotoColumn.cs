using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TechWaveOnlineShopping.Migrations
{
    /// <inheritdoc />
    public partial class RemovePhotoColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Photo",
                table: "Products");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "Photo",
                table: "Products",
                type: "varbinary(max)",
                nullable: true);
        }
    }
}
