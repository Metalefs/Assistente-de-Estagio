using Microsoft.EntityFrameworkCore.Migrations;

namespace ADE.Infra.Data.Migrations
{
    public partial class Pontuacao : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Pontuacao",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Pontuacao",
                table: "AspNetUsers");
        }
    }
}
