using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BukyBookWeb.Migrations
{
    /// <inheritdoc />
    public partial class ChangeDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ChangeDetails",
                table: "AuditLogs",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChangeDetails",
                table: "AuditLogs");
        }
    }
}
