using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CartService.Migrations
{
    /// <inheritdoc />
    public partial class RenameColums : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProductPrice",
                schema: "cart",
                table: "CartItems",
                newName: "ItemPrice");

            migrationBuilder.AddColumn<string>(
                name: "ItemName",
                schema: "cart",
                table: "CartItems",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ItemName",
                schema: "cart",
                table: "CartItems");

            migrationBuilder.RenameColumn(
                name: "ItemPrice",
                schema: "cart",
                table: "CartItems",
                newName: "ProductPrice");
        }
    }
}
