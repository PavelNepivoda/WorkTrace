using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkTrace.Migrations
{
    /// <inheritdoc />
    public partial class SystemSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AbsenceTypes",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"));

            migrationBuilder.CreateTable(
                name: "SystemSettings",
                columns: table => new
                {
                    Key = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemSettings", x => x.Key);
                });

            migrationBuilder.InsertData(
                table: "SystemSettings",
                columns: new[] { "Key", "Description", "Value" },
                values: new object[] { "WorkingDayHours", "Počet hodin v pracovním dni (pro převod absencí na hodiny)", "8" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SystemSettings");

            migrationBuilder.InsertData(
                table: "AbsenceTypes",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[] { new Guid("33333333-3333-3333-3333-333333333333"), "Práce z domova", "Home office" });
        }
    }
}
