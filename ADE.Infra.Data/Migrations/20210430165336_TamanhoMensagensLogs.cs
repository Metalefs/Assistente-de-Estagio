using Microsoft.EntityFrameworkCore.Migrations;

namespace ADE.Infra.Data.Migrations
{
    public partial class TamanhoMensagensLogs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Mensagem",
                table: "SysLogs",
                type: "text",
                nullable: false,
                defaultValueSql: "'error'",
                oldClrType: typeof(string),
                oldType: "varchar(500)",
                oldDefaultValueSql: "'error'")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Mensagem",
                table: "LogAcoesEspeciais",
                type: "text",
                nullable: false,
                defaultValueSql: "'error'",
                oldClrType: typeof(string),
                oldType: "varchar(500)",
                oldDefaultValueSql: "'error'")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Descricao",
                table: "HistoricoGeracaoDocumento",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(500)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Mensagem",
                table: "SysLogs",
                type: "varchar(500)",
                nullable: false,
                defaultValueSql: "'error'",
                oldClrType: typeof(string),
                oldType: "text",
                oldDefaultValueSql: "'error'")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Mensagem",
                table: "LogAcoesEspeciais",
                type: "varchar(500)",
                nullable: false,
                defaultValueSql: "'error'",
                oldClrType: typeof(string),
                oldType: "text",
                oldDefaultValueSql: "'error'")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Descricao",
                table: "HistoricoGeracaoDocumento",
                type: "varchar(500)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }
    }
}
