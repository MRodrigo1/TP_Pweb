using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TP_Pweb.Data.Migrations
{
    public partial class veiculoreserva : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_reservas_veiculos_VeiculoId",
                table: "reservas");

            migrationBuilder.DropIndex(
                name: "IX_reservas_VeiculoId",
                table: "reservas");

            migrationBuilder.DropColumn(
                name: "VeiculoId",
                table: "reservas");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "VeiculoId",
                table: "reservas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_reservas_VeiculoId",
                table: "reservas",
                column: "VeiculoId");

            migrationBuilder.AddForeignKey(
                name: "FK_reservas_veiculos_VeiculoId",
                table: "reservas",
                column: "VeiculoId",
                principalTable: "veiculos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
