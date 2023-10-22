using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace hr.Migrations
{
    /// <inheritdoc />
    public partial class cources : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CourseDate",
                table: "Courses",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Organization",
                table: "Courses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "OrganizationLogoUrl",
                table: "Courses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RegisterLink",
                table: "Courses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CourseDate",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "Organization",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "OrganizationLogoUrl",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "RegisterLink",
                table: "Courses");
        }
    }
}
