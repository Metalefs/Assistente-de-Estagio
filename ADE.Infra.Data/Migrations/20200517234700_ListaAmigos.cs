using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ADE.Infra.Data.Migrations
{
    public partial class ListaAmigos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ListaAmigos",
                columns: table => new
                {
                    Identificador = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DataHoraInclusao = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "current_timestamp()"),
                    DataHoraUltimaAlteracao = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "current_timestamp()"),
                    DataHoraExclusao = table.Column<DateTime>(type: "datetime", nullable: true),
                    IdUsuario = table.Column<string>(type: "varchar(50)", nullable: false),
                    IdAmigo = table.Column<string>(type: "varchar(50)", nullable: false),
                    TipoRelacao = table.Column<string>(unicode: false, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.Identificador);
                    table.ForeignKey(
                        name: "FK_ListaAmigos_AspNetUsers_IdAmigo",
                        column: x => x.IdAmigo,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ListaAmigos_IdAmigo",
                table: "ListaAmigos",
                column: "IdAmigo");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ListaAmigos");
        }
    }
}
