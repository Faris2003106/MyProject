using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace hr.Migrations
{
    /// <inheritdoc />
    public partial class Discounts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "Discounts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "FacebookUrl",
                table: "Discounts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "InsatgramUrl",
                table: "Discounts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SnapchatUrl",
                table: "Discounts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "Discounts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "TwitterUrl",
                table: "Discounts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "Discounts");

            migrationBuilder.DropColumn(
                name: "FacebookUrl",
                table: "Discounts");

            migrationBuilder.DropColumn(
                name: "InsatgramUrl",
                table: "Discounts");

            migrationBuilder.DropColumn(
                name: "SnapchatUrl",
                table: "Discounts");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "Discounts");

            migrationBuilder.DropColumn(
                name: "TwitterUrl",
                table: "Discounts");
        }
    }
}
