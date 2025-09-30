using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BukyBookWeb.Migrations
{
    /// <inheritdoc />
    public partial class test2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ChangeDetails",
                table: "AuditLogs",
                newName: "ChangeDetailsALL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ChangeDetailsALL",
                table: "AuditLogs",
                newName: "ChangeDetails");
        }
    }
}
