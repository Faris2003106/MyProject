using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace hr.Migrations
{
    /// <inheritdoc />
    public partial class HomePage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           

            migrationBuilder.CreateTable(
                name: "HomePage",
                columns: table => new
                {
                    StaticId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StaitcTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StaitcNum = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HomePage", x => x.StaticId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HomePage");

            
        }
    }
}
