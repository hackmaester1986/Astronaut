using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StargateAPI.Migrations
{
    /// <inheritdoc />
    public partial class updateTableName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ProcessLog",
                table: "ProcessLog");

            migrationBuilder.RenameTable(
                name: "ProcessLog",
                newName: "ProcessLogs");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProcessLogs",
                table: "ProcessLogs",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ProcessLogs",
                table: "ProcessLogs");

            migrationBuilder.RenameTable(
                name: "ProcessLogs",
                newName: "ProcessLog");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProcessLog",
                table: "ProcessLog",
                column: "Id");
        }
    }
}
