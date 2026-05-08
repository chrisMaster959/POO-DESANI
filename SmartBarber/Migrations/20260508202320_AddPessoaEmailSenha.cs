using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartBarber.Migrations
{
    /// <inheritdoc />
    public partial class AddPessoaEmailSenha : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Pessoa",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Senha",
                table: "Pessoa",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "Pessoa");

            migrationBuilder.DropColumn(
                name: "Senha",
                table: "Pessoa");
        }
    }
}
