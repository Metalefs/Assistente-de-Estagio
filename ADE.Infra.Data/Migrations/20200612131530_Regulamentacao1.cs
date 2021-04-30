using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ADE.Infra.Data.Migrations
{
    public partial class Regulamentacao1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IdCurso",
                table: "Curso",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "RegulamentacaoCurso",
                columns: table => new
                {
                    Identificador = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DataHoraInclusao = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "current_timestamp()"),
                    DataHoraUltimaAlteracao = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "current_timestamp()"),
                    DataHoraExclusao = table.Column<DateTime>(type: "datetime", nullable: true),
                    IdCurso = table.Column<int>(type: "int(11)", nullable: false),
                    Endereco = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.Identificador);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Curso_IdCurso",
                table: "Curso",
                column: "IdCurso",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Curso_RegulamentacaoCurso_IdCurso",
                table: "Curso",
                column: "IdCurso",
                principalTable: "RegulamentacaoCurso",
                principalColumn: "Identificador",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Curso_RegulamentacaoCurso_IdCurso",
                table: "Curso");

            migrationBuilder.DropTable(
                name: "RegulamentacaoCurso");

            migrationBuilder.DropIndex(
                name: "IX_Curso_IdCurso",
                table: "Curso");

            migrationBuilder.DropColumn(
                name: "IdCurso",
                table: "Curso");
        }
    }
}
