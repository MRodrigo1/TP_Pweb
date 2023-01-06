using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TP_Pweb.Data.Migrations
{
    public partial class changename : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Estado_AspNetUsers_UtilizadorId",
                table: "Estado");

            migrationBuilder.RenameColumn(
                name: "UtilizadorId",
                table: "Estado",
                newName: "FuncionarioId");

            migrationBuilder.RenameIndex(
                name: "IX_Estado_UtilizadorId",
                table: "Estado",
                newName: "IX_Estado_FuncionarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Estado_AspNetUsers_FuncionarioId",
                table: "Estado",
                column: "FuncionarioId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Estado_AspNetUsers_FuncionarioId",
                table: "Estado");

            migrationBuilder.RenameColumn(
                name: "FuncionarioId",
                table: "Estado",
                newName: "UtilizadorId");

            migrationBuilder.RenameIndex(
                name: "IX_Estado_FuncionarioId",
                table: "Estado",
                newName: "IX_Estado_UtilizadorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Estado_AspNetUsers_UtilizadorId",
                table: "Estado",
                column: "UtilizadorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
