using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SalesOrderAPI.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTables2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_it_items",
                table: "it_items");

            migrationBuilder.RenameTable(
                name: "it_items",
                newName: "items");

            migrationBuilder.AddPrimaryKey(
                name: "PK_items",
                table: "items",
                column: "it_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_items",
                table: "items");

            migrationBuilder.RenameTable(
                name: "items",
                newName: "it_items");

            migrationBuilder.AddPrimaryKey(
                name: "PK_it_items",
                table: "it_items",
                column: "it_id");
        }
    }
}
