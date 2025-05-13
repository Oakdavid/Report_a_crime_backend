using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Report_A_Crime.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAdminSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: new Guid("419460cf-bda5-41bd-a342-eb30c795fda3"),
                column: "Password",
                value: "$2a$11$OeBOGk5F96SCinvmzZWhIe9qc2A4bkHksn4OizlxC9r8J1TfhU4N2");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: new Guid("419460cf-bda5-41bd-a342-eb30c795fda3"),
                column: "Password",
                value: "$2a$11$LTuYUgY7mFt/8OE...");
        }
    }
}
