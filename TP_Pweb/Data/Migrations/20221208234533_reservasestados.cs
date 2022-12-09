using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TP_Pweb.Data.Migrations
{
    public partial class reservasestados : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EstadoEntregaId",
                table: "reservas",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EstadoRecolhaId",
                table: "reservas",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Estado",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NrKilometros = table.Column<int>(type: "int", nullable: false),
                    danos = table.Column<bool>(type: "bit", nullable: false),
                    ProvaDanos = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    observacoes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UtilizadorId = table.Column<int>(type: "int", nullable: false),
                    FuncionarioId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Estado", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Estado_AspNetUsers_FuncionarioId",
                        column: x => x.FuncionarioId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_reservas_EstadoEntregaId",
                table: "reservas",
                column: "EstadoEntregaId");

            migrationBuilder.CreateIndex(
                name: "IX_reservas_EstadoRecolhaId",
                table: "reservas",
                column: "EstadoRecolhaId");

            migrationBuilder.CreateIndex(
                name: "IX_Estado_FuncionarioId",
                table: "Estado",
                column: "FuncionarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_reservas_Estado_EstadoEntregaId",
                table: "reservas",
                column: "EstadoEntregaId",
                principalTable: "Estado",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_reservas_Estado_EstadoRecolhaId",
                table: "reservas",
                column: "EstadoRecolhaId",
                principalTable: "Estado",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_reservas_Estado_EstadoEntregaId",
                table: "reservas");

            migrationBuilder.DropForeignKey(
                name: "FK_reservas_Estado_EstadoRecolhaId",
                table: "reservas");

            migrationBuilder.DropTable(
                name: "Estado");

            migrationBuilder.DropIndex(
                name: "IX_reservas_EstadoEntregaId",
                table: "reservas");

            migrationBuilder.DropIndex(
                name: "IX_reservas_EstadoRecolhaId",
                table: "reservas");

            migrationBuilder.DropColumn(
                name: "EstadoEntregaId",
                table: "reservas");

            migrationBuilder.DropColumn(
                name: "EstadoRecolhaId",
                table: "reservas");
        }
    }
}
