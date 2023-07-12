using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JuanMVC.Migrations
{
    public partial class AddRatePropToProduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte>(
                name: "Rate",
                table: "Products",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rate",
                table: "Products");
        }
    }
}
