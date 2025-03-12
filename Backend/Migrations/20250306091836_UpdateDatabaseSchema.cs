using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GoodsAndOrders.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDatabaseSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_products_product_category_category_id",
                table: "products");

            migrationBuilder.DropForeignKey(
                name: "fk_products_user_orders_user_order_user_order_id",
                table: "products_user_orders");

            migrationBuilder.DropForeignKey(
                name: "fk_users_user_roles_user_role_id",
                table: "users");

            migrationBuilder.AddForeignKey(
                name: "fk_products_product_categories_category_id",
                table: "products",
                column: "category_id",
                principalTable: "product_categories",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_products_user_orders_user_orders_user_order_id",
                table: "products_user_orders",
                column: "user_order_id",
                principalTable: "user_orders",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_users_user_roles_user_role_id",
                table: "users",
                column: "user_role_id",
                principalTable: "user_roles",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_products_product_categories_category_id",
                table: "products");

            migrationBuilder.DropForeignKey(
                name: "fk_products_user_orders_user_orders_user_order_id",
                table: "products_user_orders");

            migrationBuilder.DropForeignKey(
                name: "fk_users_user_roles_user_role_id",
                table: "users");

            migrationBuilder.AddForeignKey(
                name: "fk_products_product_category_category_id",
                table: "products",
                column: "category_id",
                principalTable: "product_categories",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_products_user_orders_user_order_user_order_id",
                table: "products_user_orders",
                column: "user_order_id",
                principalTable: "user_orders",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_users_user_roles_user_role_id",
                table: "users",
                column: "user_role_id",
                principalTable: "user_roles",
                principalColumn: "id");
        }
    }
}
