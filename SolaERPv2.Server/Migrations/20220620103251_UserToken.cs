using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SolaERPv2.Server.Migrations
{
    public partial class UserToken : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "UserToken",
                schema: "Config",
                table: "AppUser",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserToken",
                schema: "Config",
                table: "AppUser");
        }
    }
}
