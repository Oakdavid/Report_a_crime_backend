using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Report_A_Crime.Migrations
{
    /// <inheritdoc />
    public partial class removedlocation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Reports",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "Reports");
        }
    }
}
