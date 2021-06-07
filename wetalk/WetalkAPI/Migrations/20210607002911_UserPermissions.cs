using Microsoft.EntityFrameworkCore.Migrations;

namespace WetalkAPI.Migrations
{
    public partial class UserPermissions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PermissionID",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "UserPermissions",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPermissions", x => x.ID);
                });

            migrationBuilder.InsertData(
                table: "UserPermissions",
                columns: new[] { "ID", "Description" },
                values: new object[] { 1, "Admin" });

            migrationBuilder.InsertData(
                table: "UserPermissions",
                columns: new[] { "ID", "Description" },
                values: new object[] { 2, "User" });

            migrationBuilder.CreateIndex(
                name: "IX_Users_PermissionID",
                table: "Users",
                column: "PermissionID",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_UserPermissions_PermissionID",
                table: "Users",
                column: "PermissionID",
                principalTable: "UserPermissions",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_UserPermissions_PermissionID",
                table: "Users");

            migrationBuilder.DropTable(
                name: "UserPermissions");

            migrationBuilder.DropIndex(
                name: "IX_Users_PermissionID",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PermissionID",
                table: "Users");
        }
    }
}
