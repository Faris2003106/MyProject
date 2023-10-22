using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace hr.Migrations
{
    /// <inheritdoc />
    public partial class event3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "status",
                table: "Courses",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "status",
                table: "Courses");
        }
    }
}
