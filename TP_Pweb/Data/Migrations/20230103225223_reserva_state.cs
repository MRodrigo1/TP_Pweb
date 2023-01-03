using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TP_Pweb.Data.Migrations
{
    public partial class reserva_state : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Concluida",
                table: "reservas");

            migrationBuilder.AddColumn<int>(
                name: "state",
                table: "reservas",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "state",
                table: "reservas");

            migrationBuilder.AddColumn<bool>(
                name: "Concluida",
                table: "reservas",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
