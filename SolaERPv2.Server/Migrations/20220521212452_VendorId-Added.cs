using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SolaERPv2.Server.Migrations
{
    public partial class VendorIdAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "VendorId",
                schema: "Config",
                table: "AppUser",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VendorId",
                schema: "Config",
                table: "AppUser");
        }
    }
}
