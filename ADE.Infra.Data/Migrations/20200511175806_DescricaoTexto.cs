using Microsoft.EntityFrameworkCore.Migrations;

namespace ADE.Infra.Data.Migrations
{
    public partial class DescricaoTexto : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Texto",
                table: "Documento",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "tinytext",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Texto",
                table: "Documento",
                type: "tinytext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }
    }
}
