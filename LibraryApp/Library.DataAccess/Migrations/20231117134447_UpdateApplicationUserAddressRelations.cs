using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Library.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class UpdateApplicationUserAddressRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "UserAddress");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "UserAddress",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "PrimaryAddressId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserAddress_ApplicationUserId",
                table: "UserAddress",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserAddress_AspNetUsers_ApplicationUserId",
                table: "UserAddress",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserAddress_AspNetUsers_ApplicationUserId",
                table: "UserAddress");

            migrationBuilder.DropIndex(
                name: "IX_UserAddress_ApplicationUserId",
                table: "UserAddress");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "UserAddress");

            migrationBuilder.DropColumn(
                name: "PrimaryAddressId",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "UserAddress",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
