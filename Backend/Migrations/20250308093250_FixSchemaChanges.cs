using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GoodsAndOrders.Migrations
{
    /// <inheritdoc />
    public partial class FixSchemaChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_products_user_orders_user_orders_user_order_id",
                table: "products_user_orders");

            migrationBuilder.AddForeignKey(
                name: "fk_products_user_orders_user_orders_user_order_id",
                table: "products_user_orders",
                column: "user_order_id",
                principalTable: "user_orders",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_products_user_orders_user_orders_user_order_id",
                table: "products_user_orders");

            migrationBuilder.AddForeignKey(
                name: "fk_products_user_orders_user_orders_user_order_id",
                table: "products_user_orders",
                column: "user_order_id",
                principalTable: "user_orders",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
