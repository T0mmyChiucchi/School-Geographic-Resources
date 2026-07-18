using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolGeoResources.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPublicationStateToPlace : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "State",
                table: "Places",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "State",
                table: "Places");
        }
    }
}
