using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InfotecsTestApplication.Migrations
{
    /// <inheritdoc />
    public partial class AddAverageExecutionTimeToResults : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "AverageExecutionTime",
                table: "Results",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AverageExecutionTime",
                table: "Results");
        }
    }
}
