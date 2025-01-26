using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SalesOrderAPI.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTablesNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "items",
                columns: table => new
                {
                    it_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    it_name = table.Column<string>(type: "text", nullable: true),
                    it_price = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_items", x => x.it_id);
                });

            migrationBuilder.CreateTable(
                name: "sales_order",
                columns: table => new
                {
                    so_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    so_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    so_total = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sales_order", x => x.so_id);
                });

            migrationBuilder.CreateTable(
                name: "order_item",
                columns: table => new
                {
                    oi_id = table.Column<Guid>(type: "uuid", nullable: false),
                    oi_salesorderid = table.Column<int>(type: "integer", nullable: false),
                    oi_itemid = table.Column<int>(type: "integer", nullable: false),
                    oi_quantity = table.Column<int>(type: "integer", nullable: false),
                    oi_price = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_order_item", x => x.oi_id);
                    table.ForeignKey(
                        name: "FK_order_item_sales_order_oi_salesorderid",
                        column: x => x.oi_salesorderid,
                        principalTable: "sales_order",
                        principalColumn: "so_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_order_item_oi_salesorderid",
                table: "order_item",
                column: "oi_salesorderid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "items");

            migrationBuilder.DropTable(
                name: "order_item");

            migrationBuilder.DropTable(
                name: "sales_order");
        }
    }
}
