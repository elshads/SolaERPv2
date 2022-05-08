using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SolaERPv2.Server.Migrations
{
    public partial class UserPositionAndCompany : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                schema: "Config",
                table: "AppUser",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Position",
                schema: "Config",
                table: "AppUser",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompanyId",
                schema: "Config",
                table: "AppUser");

            migrationBuilder.DropColumn(
                name: "Position",
                schema: "Config",
                table: "AppUser");
        }
    }
}
