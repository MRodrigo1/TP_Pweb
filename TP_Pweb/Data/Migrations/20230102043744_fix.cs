using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TP_Pweb.Data.Migrations
{
    public partial class fix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_reservas_AspNetUsers_UtilizadorId",
                table: "reservas");

            migrationBuilder.DropIndex(
                name: "IX_reservas_UtilizadorId",
                table: "reservas");

            migrationBuilder.AlterColumn<int>(
                name: "UtilizadorId",
                table: "reservas",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UtilizadorId1",
                table: "reservas",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_reservas_UtilizadorId1",
                table: "reservas",
                column: "UtilizadorId1");

            migrationBuilder.AddForeignKey(
                name: "FK_reservas_AspNetUsers_UtilizadorId1",
                table: "reservas",
                column: "UtilizadorId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_reservas_AspNetUsers_UtilizadorId1",
                table: "reservas");

            migrationBuilder.DropIndex(
                name: "IX_reservas_UtilizadorId1",
                table: "reservas");

            migrationBuilder.DropColumn(
                name: "UtilizadorId1",
                table: "reservas");

            migrationBuilder.AlterColumn<string>(
                name: "UtilizadorId",
                table: "reservas",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_reservas_UtilizadorId",
                table: "reservas",
                column: "UtilizadorId");

            migrationBuilder.AddForeignKey(
                name: "FK_reservas_AspNetUsers_UtilizadorId",
                table: "reservas",
                column: "UtilizadorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
