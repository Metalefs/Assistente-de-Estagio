using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ADE.Infra.Data.Migrations
{
    public partial class ArquivoPDF : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "ArquivoPDF",
                table: "Documento",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ArquivoPDF",
                table: "Documento");
        }
    }
}
