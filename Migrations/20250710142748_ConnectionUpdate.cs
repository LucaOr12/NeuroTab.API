using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace neurotab_api.Migrations
{
    /// <inheritdoc />
    public partial class ConnectionUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Connections_Tabs_FromTabId",
                table: "Connections");

            migrationBuilder.DropForeignKey(
                name: "FK_Connections_Tabs_ToTabId",
                table: "Connections");

            migrationBuilder.RenameColumn(
                name: "ToTabId",
                table: "Connections",
                newName: "ToContentId");

            migrationBuilder.RenameColumn(
                name: "FromTabId",
                table: "Connections",
                newName: "FromContentId");

            migrationBuilder.RenameIndex(
                name: "IX_Connections_ToTabId",
                table: "Connections",
                newName: "IX_Connections_ToContentId");

            migrationBuilder.RenameIndex(
                name: "IX_Connections_FromTabId",
                table: "Connections",
                newName: "IX_Connections_FromContentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Connections_Contents_FromContentId",
                table: "Connections",
                column: "FromContentId",
                principalTable: "Contents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Connections_Contents_ToContentId",
                table: "Connections",
                column: "ToContentId",
                principalTable: "Contents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Connections_Contents_FromContentId",
                table: "Connections");

            migrationBuilder.DropForeignKey(
                name: "FK_Connections_Contents_ToContentId",
                table: "Connections");

            migrationBuilder.RenameColumn(
                name: "ToContentId",
                table: "Connections",
                newName: "ToTabId");

            migrationBuilder.RenameColumn(
                name: "FromContentId",
                table: "Connections",
                newName: "FromTabId");

            migrationBuilder.RenameIndex(
                name: "IX_Connections_ToContentId",
                table: "Connections",
                newName: "IX_Connections_ToTabId");

            migrationBuilder.RenameIndex(
                name: "IX_Connections_FromContentId",
                table: "Connections",
                newName: "IX_Connections_FromTabId");

            migrationBuilder.AddForeignKey(
                name: "FK_Connections_Tabs_FromTabId",
                table: "Connections",
                column: "FromTabId",
                principalTable: "Tabs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Connections_Tabs_ToTabId",
                table: "Connections",
                column: "ToTabId",
                principalTable: "Tabs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
