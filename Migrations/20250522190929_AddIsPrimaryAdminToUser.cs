using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Report_A_Crime.Migrations
{
    /// <inheritdoc />
    public partial class AddIsPrimaryAdminToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPrimaryAdmin",
                table: "Users",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: new Guid("419460cf-bda5-41bd-a342-eb30c795fda3"),
                column: "IsPrimaryAdmin",
                value: false);

            migrationBuilder.CreateIndex(
                name: "IX_SharedWithUs_ReportId",
                table: "SharedWithUs",
                column: "ReportId");

            migrationBuilder.AddForeignKey(
                name: "FK_SharedWithUs_Reports_ReportId",
                table: "SharedWithUs",
                column: "ReportId",
                principalTable: "Reports",
                principalColumn: "ReportId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SharedWithUs_Reports_ReportId",
                table: "SharedWithUs");

            migrationBuilder.DropIndex(
                name: "IX_SharedWithUs_ReportId",
                table: "SharedWithUs");

            migrationBuilder.DropColumn(
                name: "IsPrimaryAdmin",
                table: "Users");
        }
    }
}
