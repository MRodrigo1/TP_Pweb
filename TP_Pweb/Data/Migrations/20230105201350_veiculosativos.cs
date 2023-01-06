using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TP_Pweb.Data.Migrations
{
    public partial class veiculosativos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "danos2",
                table: "Estado");

            migrationBuilder.AddColumn<bool>(
                name: "ativo",
                table: "veiculos",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ativo",
                table: "veiculos");

            migrationBuilder.AddColumn<bool>(
                name: "danos2",
                table: "Estado",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
