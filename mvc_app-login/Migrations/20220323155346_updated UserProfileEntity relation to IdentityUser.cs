using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace mvc_app_login.Migrations
{
    public partial class updatedUserProfileEntityrelationtoIdentityUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserProfiles_AspNetUsers_UserFromIdentityId",
                table: "UserProfiles");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "UserProfiles");

            migrationBuilder.AlterColumn<string>(
                name: "UserFromIdentityId",
                table: "UserProfiles",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_UserProfiles_AspNetUsers_UserFromIdentityId",
                table: "UserProfiles",
                column: "UserFromIdentityId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserProfiles_AspNetUsers_UserFromIdentityId",
                table: "UserProfiles");

            migrationBuilder.AlterColumn<string>(
                name: "UserFromIdentityId",
                table: "UserProfiles",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "UserProfiles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_UserProfiles_AspNetUsers_UserFromIdentityId",
                table: "UserProfiles",
                column: "UserFromIdentityId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
