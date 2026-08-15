using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace InfotecsTestApplication.Migrations
{
    /// <inheritdoc />
    public partial class AddResult : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ResultId",
                table: "Values",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "ResultModel",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResultModel", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Values_ResultId",
                table: "Values",
                column: "ResultId");

            migrationBuilder.AddForeignKey(
                name: "FK_Values_ResultModel_ResultId",
                table: "Values",
                column: "ResultId",
                principalTable: "ResultModel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Values_ResultModels_ResultId",
                table: "Values");

            migrationBuilder.DropTable(
                name: "ResultModel");

            migrationBuilder.DropIndex(
                name: "IX_Values_ResultId",
                table: "Values");

            migrationBuilder.DropColumn(
                name: "ResultId",
                table: "Values");
        }
    }
}
