using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ADE.Infra.Data.Migrations
{
    public partial class Formulario : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "InText",
                table: "Requisito",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Size",
                table: "Requisito",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "AreaEstagioCurso",
                columns: table => new
                {
                    Identificador = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DataHoraInclusao = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "current_timestamp()"),
                    DataHoraUltimaAlteracao = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "current_timestamp()"),
                    DataHoraExclusao = table.Column<DateTime>(type: "datetime", nullable: true),
                    IdCurso = table.Column<int>(nullable: false),
                    Nome = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.Identificador);
                    table.ForeignKey(
                        name: "UKIdAreasEstagio_Curso",
                        column: x => x.IdCurso,
                        principalTable: "Curso",
                        principalColumn: "Identificador",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AreaEstagioCurso_IdCurso",
                table: "AreaEstagioCurso",
                column: "IdCurso");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AreaEstagioCurso");

            migrationBuilder.DropColumn(
                name: "InText",
                table: "Requisito");

            migrationBuilder.DropColumn(
                name: "Size",
                table: "Requisito");
        }
    }
}
