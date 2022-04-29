using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SolaERPv2.Server.Migrations
{
    public partial class UserTypeId_plus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserTypeId",
                schema: "Config",
                table: "AppUser",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //
        }
    }
}
