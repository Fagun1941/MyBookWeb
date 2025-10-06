using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BukyBookWeb.Migrations
{
    /// <inheritdoc />
    public partial class addnewfieldcreatebytimeupdatebytime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "createby",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "createtime",
                table: "Products",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "updateTime",
                table: "Products",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "updateby",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "createby",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "createtime",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "updateTime",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "updateby",
                table: "Products");
        }
    }
}
