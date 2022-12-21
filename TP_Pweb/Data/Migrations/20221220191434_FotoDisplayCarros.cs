using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TP_Pweb.Data.Migrations
{
    public partial class FotoDisplayCarros : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_reservas_AspNetUsers_UtilizadorId",
                table: "reservas");

            migrationBuilder.DropForeignKey(
                name: "FK_reservas_veiculos_VeiculoId",
                table: "reservas");

            migrationBuilder.AddColumn<byte[]>(
                name: "FotoDisplay",
                table: "veiculos",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "VeiculoId",
                table: "reservas",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "UtilizadorId",
                table: "reservas",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_reservas_AspNetUsers_UtilizadorId",
                table: "reservas",
                column: "UtilizadorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_reservas_veiculos_VeiculoId",
                table: "reservas",
                column: "VeiculoId",
                principalTable: "veiculos",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_reservas_AspNetUsers_UtilizadorId",
                table: "reservas");

            migrationBuilder.DropForeignKey(
                name: "FK_reservas_veiculos_VeiculoId",
                table: "reservas");

            migrationBuilder.DropColumn(
                name: "FotoDisplay",
                table: "veiculos");

            migrationBuilder.AlterColumn<int>(
                name: "VeiculoId",
                table: "reservas",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UtilizadorId",
                table: "reservas",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_reservas_AspNetUsers_UtilizadorId",
                table: "reservas",
                column: "UtilizadorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

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
