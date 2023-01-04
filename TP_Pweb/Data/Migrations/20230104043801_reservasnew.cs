using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TP_Pweb.Data.Migrations
{
    public partial class reservasnew : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_reservas_Estado_EstadoEntregaId",
                table: "reservas");

            migrationBuilder.DropForeignKey(
                name: "FK_reservas_Estado_EstadoRecolhaId",
                table: "reservas");

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

            migrationBuilder.DropColumn(
                name: "Concluido",
                table: "Estado");

            migrationBuilder.AddColumn<int>(
                name: "ReservaId",
                table: "Estado",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "state",
                table: "Estado",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Estado_ReservaId",
                table: "Estado",
                column: "ReservaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Estado_reservas_ReservaId",
                table: "Estado",
                column: "ReservaId",
                principalTable: "reservas",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Estado_reservas_ReservaId",
                table: "Estado");

            migrationBuilder.DropIndex(
                name: "IX_Estado_ReservaId",
                table: "Estado");

            migrationBuilder.DropColumn(
                name: "ReservaId",
                table: "Estado");

            migrationBuilder.DropColumn(
                name: "state",
                table: "Estado");

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

            migrationBuilder.AddColumn<bool>(
                name: "Concluido",
                table: "Estado",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_reservas_EstadoEntregaId",
                table: "reservas",
                column: "EstadoEntregaId");

            migrationBuilder.CreateIndex(
                name: "IX_reservas_EstadoRecolhaId",
                table: "reservas",
                column: "EstadoRecolhaId");

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
    }
}
