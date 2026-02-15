using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CartService.Migrations
{
    /// <inheritdoc />
    public partial class renameFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                schema: "cart",
                table: "UserCarts",
                newName: "updatedAt");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                schema: "cart",
                table: "UserCarts",
                newName: "createdAt");

            migrationBuilder.RenameColumn(
                name: "Id",
                schema: "cart",
                table: "UserCarts",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                schema: "cart",
                table: "CartItems",
                newName: "updatedAt");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                schema: "cart",
                table: "CartItems",
                newName: "createdAt");

            migrationBuilder.RenameColumn(
                name: "Id",
                schema: "cart",
                table: "CartItems",
                newName: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "updatedAt",
                schema: "cart",
                table: "UserCarts",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "createdAt",
                schema: "cart",
                table: "UserCarts",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "id",
                schema: "cart",
                table: "UserCarts",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "updatedAt",
                schema: "cart",
                table: "CartItems",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "createdAt",
                schema: "cart",
                table: "CartItems",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "id",
                schema: "cart",
                table: "CartItems",
                newName: "Id");
        }
    }
}
