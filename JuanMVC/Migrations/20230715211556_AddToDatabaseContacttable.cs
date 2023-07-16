using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JuanMVC.Migrations
{
    public partial class AddToDatabaseContacttable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserContact_AspNetUsers_AppUserId",
                table: "UserContact");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserContact",
                table: "UserContact");

            migrationBuilder.RenameTable(
                name: "UserContact",
                newName: "UserContacts");

            migrationBuilder.RenameIndex(
                name: "IX_UserContact_AppUserId",
                table: "UserContacts",
                newName: "IX_UserContacts_AppUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserContacts",
                table: "UserContacts",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserContacts_AspNetUsers_AppUserId",
                table: "UserContacts",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserContacts_AspNetUsers_AppUserId",
                table: "UserContacts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserContacts",
                table: "UserContacts");

            migrationBuilder.RenameTable(
                name: "UserContacts",
                newName: "UserContact");

            migrationBuilder.RenameIndex(
                name: "IX_UserContacts_AppUserId",
                table: "UserContact",
                newName: "IX_UserContact_AppUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserContact",
                table: "UserContact",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserContact_AspNetUsers_AppUserId",
                table: "UserContact",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
