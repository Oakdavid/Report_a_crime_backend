using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Report_A_Crime.Migrations
{
    /// <inheritdoc />
    public partial class IsPrimaryAdminToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: new Guid("419460cf-bda5-41bd-a342-eb30c795fda3"),
                column: "IsPrimaryAdmin",
                value: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: new Guid("419460cf-bda5-41bd-a342-eb30c795fda3"),
                column: "IsPrimaryAdmin",
                value: false);
        }
    }
}
