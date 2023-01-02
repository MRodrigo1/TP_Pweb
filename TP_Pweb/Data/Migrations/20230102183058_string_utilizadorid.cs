using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TP_Pweb.Data.Migrations
{
    public partial class string_utilizadorid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Estado_AspNetUsers_FuncionarioId",
                table: "Estado");

            migrationBuilder.DropForeignKey(
                name: "FK_reservas_AspNetUsers_UtilizadorId1",
                table: "reservas");

            migrationBuilder.DropForeignKey(
                name: "FK_reservas_veiculos_VeiculoId",
                table: "reservas");

            migrationBuilder.DropIndex(
                name: "IX_reservas_UtilizadorId1",
                table: "reservas");

            migrationBuilder.DropIndex(
                name: "IX_Estado_FuncionarioId",
                table: "Estado");

            migrationBuilder.DropColumn(
                name: "UtilizadorId1",
                table: "reservas");

            migrationBuilder.DropColumn(
                name: "FuncionarioId",
                table: "Estado");

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
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UtilizadorId",
                table: "Estado",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_reservas_UtilizadorId",
                table: "reservas",
                column: "UtilizadorId");

            migrationBuilder.CreateIndex(
                name: "IX_Estado_UtilizadorId",
                table: "Estado",
                column: "UtilizadorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Estado_AspNetUsers_UtilizadorId",
                table: "Estado",
                column: "UtilizadorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Estado_AspNetUsers_UtilizadorId",
                table: "Estado");

            migrationBuilder.DropForeignKey(
                name: "FK_reservas_AspNetUsers_UtilizadorId",
                table: "reservas");

            migrationBuilder.DropForeignKey(
                name: "FK_reservas_veiculos_VeiculoId",
                table: "reservas");

            migrationBuilder.DropIndex(
                name: "IX_reservas_UtilizadorId",
                table: "reservas");

            migrationBuilder.DropIndex(
                name: "IX_Estado_UtilizadorId",
                table: "Estado");

            migrationBuilder.AlterColumn<int>(
                name: "VeiculoId",
                table: "reservas",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "UtilizadorId",
                table: "reservas",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "UtilizadorId1",
                table: "reservas",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "UtilizadorId",
                table: "Estado",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FuncionarioId",
                table: "Estado",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_reservas_UtilizadorId1",
                table: "reservas",
                column: "UtilizadorId1");

            migrationBuilder.CreateIndex(
                name: "IX_Estado_FuncionarioId",
                table: "Estado",
                column: "FuncionarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Estado_AspNetUsers_FuncionarioId",
                table: "Estado",
                column: "FuncionarioId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_reservas_AspNetUsers_UtilizadorId1",
                table: "reservas",
                column: "UtilizadorId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_reservas_veiculos_VeiculoId",
                table: "reservas",
                column: "VeiculoId",
                principalTable: "veiculos",
                principalColumn: "Id");
        }
    }
}
