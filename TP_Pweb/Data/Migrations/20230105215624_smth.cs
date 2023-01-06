using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TP_Pweb.Data.Migrations
{
    public partial class smth : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Estado_AspNetUsers_FuncionarioId1",
                table: "Estado");

            migrationBuilder.DropIndex(
                name: "IX_Estado_FuncionarioId1",
                table: "Estado");

            migrationBuilder.DropColumn(
                name: "FuncionarioId1",
                table: "Estado");

            migrationBuilder.AlterColumn<string>(
                name: "FuncionarioId",
                table: "Estado",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Estado_FuncionarioId",
                table: "Estado",
                column: "FuncionarioId");

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

            migrationBuilder.DropIndex(
                name: "IX_Estado_FuncionarioId",
                table: "Estado");

            migrationBuilder.AlterColumn<int>(
                name: "FuncionarioId",
                table: "Estado",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FuncionarioId1",
                table: "Estado",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Estado_FuncionarioId1",
                table: "Estado",
                column: "FuncionarioId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Estado_AspNetUsers_FuncionarioId1",
                table: "Estado",
                column: "FuncionarioId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
