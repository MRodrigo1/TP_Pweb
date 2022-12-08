using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TP_Pweb.Data.Migrations
{
    public partial class reservasRelacoes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UtilizadorId",
                table: "reservas",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "VeiculoId",
                table: "reservas",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "VeiculoId1",
                table: "reservas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<byte[]>(
                name: "Avatar",
                table: "AspNetUsers",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DataNascimento",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "NIF",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PrimeiroNome",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UltimoNome",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_reservas_UtilizadorId",
                table: "reservas",
                column: "UtilizadorId");

            migrationBuilder.CreateIndex(
                name: "IX_reservas_VeiculoId1",
                table: "reservas",
                column: "VeiculoId1");

            migrationBuilder.AddForeignKey(
                name: "FK_reservas_AspNetUsers_UtilizadorId",
                table: "reservas",
                column: "UtilizadorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_reservas_veiculos_VeiculoId1",
                table: "reservas",
                column: "VeiculoId1",
                principalTable: "veiculos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_reservas_AspNetUsers_UtilizadorId",
                table: "reservas");

            migrationBuilder.DropForeignKey(
                name: "FK_reservas_veiculos_VeiculoId1",
                table: "reservas");

            migrationBuilder.DropIndex(
                name: "IX_reservas_UtilizadorId",
                table: "reservas");

            migrationBuilder.DropIndex(
                name: "IX_reservas_VeiculoId1",
                table: "reservas");

            migrationBuilder.DropColumn(
                name: "UtilizadorId",
                table: "reservas");

            migrationBuilder.DropColumn(
                name: "VeiculoId",
                table: "reservas");

            migrationBuilder.DropColumn(
                name: "VeiculoId1",
                table: "reservas");

            migrationBuilder.DropColumn(
                name: "Avatar",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "DataNascimento",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "NIF",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PrimeiroNome",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "UltimoNome",
                table: "AspNetUsers");
        }
    }
}
