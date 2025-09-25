using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StargateAPI.Migrations
{
    /// <inheritdoc />
    public partial class addProcessLog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CareerStartDate",
                table: "AstronautDetail",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "ProcessLog",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Timestamp = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    Level = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    Message = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    StackTrace = table.Column<string>(type: "TEXT", nullable: true),
                    Context = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcessLog", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProcessLog");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CareerStartDate",
                table: "AstronautDetail",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "TEXT");
        }
    }
}
