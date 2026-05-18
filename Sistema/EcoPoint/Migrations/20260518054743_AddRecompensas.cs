using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EcoPoint.Migrations
{
    /// <inheritdoc />
    public partial class AddRecompensas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Recompensas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PontosNecessarios = table.Column<int>(type: "int", nullable: false),
                    Imagem = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recompensas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Resgates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    RecompensaId = table.Column<int>(type: "int", nullable: false),
                    DataResgate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Resgates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Resgates_Recompensas_RecompensaId",
                        column: x => x.RecompensaId,
                        principalTable: "Recompensas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Resgates_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Resgates_RecompensaId",
                table: "Resgates",
                column: "RecompensaId");

            migrationBuilder.CreateIndex(
                name: "IX_Resgates_UsuarioId",
                table: "Resgates",
                column: "UsuarioId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Resgates");

            migrationBuilder.DropTable(
                name: "Recompensas");
        }
    }
}
