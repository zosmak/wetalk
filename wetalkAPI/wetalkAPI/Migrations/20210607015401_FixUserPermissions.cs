using Microsoft.EntityFrameworkCore.Migrations;

namespace WetalkAPI.Migrations
{
    public partial class FixUserPermissions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_PermissionID",
                table: "Users");

            migrationBuilder.CreateIndex(
                name: "IX_Users_PermissionID",
                table: "Users",
                column: "PermissionID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_PermissionID",
                table: "Users");

            migrationBuilder.CreateIndex(
                name: "IX_Users_PermissionID",
                table: "Users",
                column: "PermissionID",
                unique: true);
        }
    }
}
