using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sistema_HelpDesk.Migrations
{
    /// <inheritdoc />
    public partial class CriandoAtributoPerfilChat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TipoPerfil",
                table: "Chat",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TipoPerfil",
                table: "Chat");
        }
    }
}
