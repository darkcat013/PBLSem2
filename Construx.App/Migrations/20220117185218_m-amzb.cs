using Microsoft.EntityFrameworkCore.Migrations;

namespace Construx.App.Migrations
{
    public partial class mamzb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Path",
                table: "Photos",
                newName: "Name");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Photos",
                newName: "Path");
        }
    }
}
