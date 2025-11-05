using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sistema_HelpDesk.Migrations
{
    /// <inheritdoc />
    public partial class CriandoAtributoChamadoPrioridade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "TecnicoFinalizacaoId",
                table: "Chamados",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DataFinalizacao",
                table: "Chamados",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<string>(
                name: "ComentarioFinalizacao",
                table: "Chamados",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "Prioridade",
                table: "Chamados",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TecnicoAberturaChamadoId",
                table: "Chamados",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Prioridade",
                table: "Categoria",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Chamados_TecnicoAberturaChamadoId",
                table: "Chamados",
                column: "TecnicoAberturaChamadoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Chamados_Tecnicos_TecnicoAberturaChamadoId",
                table: "Chamados",
                column: "TecnicoAberturaChamadoId",
                principalTable: "Tecnicos",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Chamados_Tecnicos_TecnicoAberturaChamadoId",
                table: "Chamados");

            migrationBuilder.DropIndex(
                name: "IX_Chamados_TecnicoAberturaChamadoId",
                table: "Chamados");

            migrationBuilder.DropColumn(
                name: "Prioridade",
                table: "Chamados");

            migrationBuilder.DropColumn(
                name: "TecnicoAberturaChamadoId",
                table: "Chamados");

            migrationBuilder.DropColumn(
                name: "Prioridade",
                table: "Categoria");

            migrationBuilder.AlterColumn<int>(
                name: "TecnicoFinalizacaoId",
                table: "Chamados",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DataFinalizacao",
                table: "Chamados",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ComentarioFinalizacao",
                table: "Chamados",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
