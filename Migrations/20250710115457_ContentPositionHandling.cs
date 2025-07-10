using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace neurotab_api.Migrations
{
    /// <inheritdoc />
    public partial class ContentPositionHandling : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "PositionX",
                table: "Contents",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "PositionY",
                table: "Contents",
                type: "real",
                nullable: false,
                defaultValue: 0f);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PositionX",
                table: "Contents");

            migrationBuilder.DropColumn(
                name: "PositionY",
                table: "Contents");
        }
    }
}
