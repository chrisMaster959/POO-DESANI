using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartBarber.Migrations
{
    /// <inheritdoc />
    public partial class AtendimentoServicoRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ServicosIds",
                table: "Atendimento");

            migrationBuilder.CreateTable(
                name: "AtendimentoServico",
                columns: table => new
                {
                    AtendimentosId = table.Column<int>(type: "int", nullable: false),
                    ServicosId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AtendimentoServico", x => new { x.AtendimentosId, x.ServicosId });
                    table.ForeignKey(
                        name: "FK_AtendimentoServico_Atendimento_AtendimentosId",
                        column: x => x.AtendimentosId,
                        principalTable: "Atendimento",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AtendimentoServico_Servico_ServicosId",
                        column: x => x.ServicosId,
                        principalTable: "Servico",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AtendimentoServico_ServicosId",
                table: "AtendimentoServico",
                column: "ServicosId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AtendimentoServico");

            migrationBuilder.AddColumn<string>(
                name: "ServicosIds",
                table: "Atendimento",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
