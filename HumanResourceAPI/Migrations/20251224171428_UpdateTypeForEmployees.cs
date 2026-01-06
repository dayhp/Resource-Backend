using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HumanResourceAPI.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTypeForEmployees : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: new Guid("0a42736d-9ab3-47e0-b993-29f7e75a5964"));

            migrationBuilder.DeleteData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: new Guid("c5878f18-6cb6-43eb-a5f9-f13c794391cb"));

            migrationBuilder.AlterColumn<int>(
                name: "Age",
                table: "Employees",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.InsertData(
                table: "Companies",
                columns: new[] { "Id", "Address", "Country", "Name" },
                values: new object[,]
                {
                    { new Guid("648fdb24-fcbe-4360-ad0d-e167c3d22d76"), "456 Innovation Road, New York, NY", "USA", "Global Innovations Inc." },
                    { new Guid("7f51984e-4b7c-4f75-a094-48572bfb50bd"), "123 Tech Avenue, Silicon Valley, CA", "USA", "Tech Solutions Ltd." }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: new Guid("648fdb24-fcbe-4360-ad0d-e167c3d22d76"));

            migrationBuilder.DeleteData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: new Guid("7f51984e-4b7c-4f75-a094-48572bfb50bd"));

            migrationBuilder.AlterColumn<string>(
                name: "Age",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.InsertData(
                table: "Companies",
                columns: new[] { "Id", "Address", "Country", "Name" },
                values: new object[,]
                {
                    { new Guid("0a42736d-9ab3-47e0-b993-29f7e75a5964"), "123 Tech Avenue, Silicon Valley, CA", "USA", "Tech Solutions Ltd." },
                    { new Guid("c5878f18-6cb6-43eb-a5f9-f13c794391cb"), "456 Innovation Road, New York, NY", "USA", "Global Innovations Inc." }
                });
        }
    }
}
