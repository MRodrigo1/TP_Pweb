using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TP_Pweb.Data.Migrations
{
    public partial class migracaoInicial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "categorias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_categorias", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Empresa",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Empresa", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "reservas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Concluida = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_reservas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "veiculos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Localizacao = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    custo = table.Column<int>(type: "int", nullable: false),
                    nrKm = table.Column<int>(type: "int", nullable: false),
                    EmpresaId = table.Column<int>(type: "int", nullable: true),
                    CategoriaId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_veiculos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_veiculos_categorias_CategoriaId",
                        column: x => x.CategoriaId,
                        principalTable: "categorias",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_veiculos_Empresa_EmpresaId",
                        column: x => x.EmpresaId,
                        principalTable: "Empresa",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_veiculos_CategoriaId",
                table: "veiculos",
                column: "CategoriaId");

            migrationBuilder.CreateIndex(
                name: "IX_veiculos_EmpresaId",
                table: "veiculos",
                column: "EmpresaId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "reservas");

            migrationBuilder.DropTable(
                name: "veiculos");

            migrationBuilder.DropTable(
                name: "categorias");

            migrationBuilder.DropTable(
                name: "Empresa");
        }
    }
}
