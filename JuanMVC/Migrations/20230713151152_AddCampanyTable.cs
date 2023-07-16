using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JuanMVC.Migrations
{
    public partial class AddCampanyTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Campanies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CampanyTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DisPrice = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CampanyDes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BacgroundImg = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Campanies", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Campanies");
        }
    }
}
