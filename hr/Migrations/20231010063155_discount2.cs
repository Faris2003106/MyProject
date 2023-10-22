using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace hr.Migrations
{
    /// <inheritdoc />
    public partial class discount2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "status",
                table: "Discounts",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "status",
                table: "Discounts");
        }
    }
}
