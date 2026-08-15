using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InfotecsTestApplication.Migrations
{
    /// <inheritdoc />
    public partial class AddedValuesToResults : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Values_ResultModel_ResultId",
                table: "Values");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ResultModel",
                table: "ResultModel");

            migrationBuilder.RenameTable(
                name: "ResultModel",
                newName: "Results");

            migrationBuilder.AddColumn<double>(
                name: "AverageValue",
                table: "Results",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<long>(
                name: "DurationSecons",
                table: "Results",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<double>(
                name: "MaxValue",
                table: "Results",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "MedianValue",
                table: "Results",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "MinValue",
                table: "Results",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartedAt",
                table: "Results",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Results",
                table: "Results",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Values_Results_ResultId",
                table: "Values",
                column: "ResultId",
                principalTable: "Results",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Values_Results_ResultId",
                table: "Values");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Results",
                table: "Results");

            migrationBuilder.DropColumn(
                name: "AverageValue",
                table: "Results");

            migrationBuilder.DropColumn(
                name: "DurationSecons",
                table: "Results");

            migrationBuilder.DropColumn(
                name: "MaxValue",
                table: "Results");

            migrationBuilder.DropColumn(
                name: "MedianValue",
                table: "Results");

            migrationBuilder.DropColumn(
                name: "MinValue",
                table: "Results");

            migrationBuilder.DropColumn(
                name: "StartedAt",
                table: "Results");

            migrationBuilder.RenameTable(
                name: "Results",
                newName: "ResultModel");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ResultModel",
                table: "ResultModel",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Values_ResultModel_ResultId",
                table: "Values",
                column: "ResultId",
                principalTable: "ResultModel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
