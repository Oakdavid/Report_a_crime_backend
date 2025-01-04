using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Report_A_Crime.Migrations
{
    /// <inheritdoc />
    public partial class geolocationupdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Geolocations",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "Geolocations",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Latitude",
                table: "Geolocations",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Longitude",
                table: "Geolocations",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "RoleId", "RoleName" },
                values: new object[] { new Guid("0504ea46-35aa-4949-9c59-b8b32a083ef6"), "User" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: new Guid("0504ea46-35aa-4949-9c59-b8b32a083ef6"));

            migrationBuilder.DropColumn(
                name: "City",
                table: "Geolocations");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "Geolocations");

            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "Geolocations");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "Geolocations");
        }
    }
}
