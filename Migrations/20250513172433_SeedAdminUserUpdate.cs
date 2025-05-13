using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Report_A_Crime.Migrations
{
    /// <inheritdoc />
    public partial class SeedAdminUserUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: new Guid("419460cf-bda5-41bd-a342-eb30c795fda3"),
                columns: new[] { "HashSalt", "Password" },
                values: new object[] { "", "$2a$11$LTuYUgY7mFt/8OE..." });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: new Guid("419460cf-bda5-41bd-a342-eb30c795fda3"),
                columns: new[] { "HashSalt", "Password" },
                values: new object[] { "Admin", null });
        }
    }
}
