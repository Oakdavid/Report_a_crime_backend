using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Report_A_Crime.Migrations
{
    /// <inheritdoc />
    public partial class addedregionetc : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "Reports");

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "Geolocations",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Region",
                table: "Geolocations",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Location",
                table: "Geolocations");

            migrationBuilder.DropColumn(
                name: "Region",
                table: "Geolocations");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Reports",
                type: "text",
                nullable: true);
        }
    }
}
