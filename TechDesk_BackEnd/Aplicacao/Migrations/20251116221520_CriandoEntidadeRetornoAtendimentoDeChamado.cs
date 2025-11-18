using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sistema_HelpDesk.Migrations
{
    /// <inheritdoc />
    public partial class CriandoEntidadeRetornoAtendimentoDeChamado : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RetornoAtendimento",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChamadoId = table.Column<int>(type: "int", nullable: false),
                    NumeroChamado = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RetornoAcao = table.Column<int>(type: "int", nullable: false),
                    NomeTecnico = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HoraAtendimento = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Mensagem = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RetornoAtendimento", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RetornoAtendimento_Chamados_ChamadoId",
                        column: x => x.ChamadoId,
                        principalTable: "Chamados",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RetornoAtendimento_ChamadoId",
                table: "RetornoAtendimento",
                column: "ChamadoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RetornoAtendimento");
        }
    }
}
