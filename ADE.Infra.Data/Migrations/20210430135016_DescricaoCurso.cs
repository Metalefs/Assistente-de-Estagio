using Microsoft.EntityFrameworkCore.Migrations;

namespace ADE.Infra.Data.Migrations
{
    public partial class DescricaoCurso : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_RequisitoDeUsuario_Identificador_IdRequisito_UserId",
                table: "RequisitoDeUsuario");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_RequisitoDeDocumento_IdDocumento_Identificador_IdRequisito",
                table: "RequisitoDeDocumento");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_OpcaoRequisito_Identificador_IdRequisito_Valor",
                table: "OpcaoRequisito");

            migrationBuilder.AlterColumn<string>(
                name: "DescricaoCurso",
                table: "Curso",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "tinytext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "DescricaoCurso",
                table: "Curso",
                type: "tinytext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_RequisitoDeUsuario_Identificador_IdRequisito_UserId",
                table: "RequisitoDeUsuario",
                columns: new[] { "Identificador", "IdRequisito", "UserId" });

            migrationBuilder.AddUniqueConstraint(
                name: "AK_RequisitoDeDocumento_IdDocumento_Identificador_IdRequisito",
                table: "RequisitoDeDocumento",
                columns: new[] { "IdDocumento", "Identificador", "IdRequisito" });

            migrationBuilder.AddUniqueConstraint(
                name: "AK_OpcaoRequisito_Identificador_IdRequisito_Valor",
                table: "OpcaoRequisito",
                columns: new[] { "Identificador", "IdRequisito", "Valor" });
        }
    }
}
