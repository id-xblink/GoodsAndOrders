using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GoodsAndOrders.Migrations
{
    /// <inheritdoc />
    public partial class RenameTablesToSnakeCase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_products_product_categories_CategoryId",
                table: "products");

            migrationBuilder.DropForeignKey(
                name: "FK_products_user_orders_products_ProductId",
                table: "products_user_orders");

            migrationBuilder.DropForeignKey(
                name: "FK_products_user_orders_user_orders_UserOrderId",
                table: "products_user_orders");

            migrationBuilder.DropForeignKey(
                name: "FK_user_orders_order_statuses_StatusId",
                table: "user_orders");

            migrationBuilder.DropForeignKey(
                name: "FK_user_orders_users_CustomerId",
                table: "user_orders");

            migrationBuilder.DropForeignKey(
                name: "FK_users_user_roles_UserRoleId",
                table: "users");

            migrationBuilder.DropForeignKey(
                name: "FK_users_users_CustomerId",
                table: "users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_users",
                table: "users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_user_roles",
                table: "user_roles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_user_orders",
                table: "user_orders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_products_user_orders",
                table: "products_user_orders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_products",
                table: "products");

            migrationBuilder.DropPrimaryKey(
                name: "PK_product_categories",
                table: "product_categories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_order_statuses",
                table: "order_statuses");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "users",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Discount",
                table: "users",
                newName: "discount");

            migrationBuilder.RenameColumn(
                name: "Code",
                table: "users",
                newName: "code");

            migrationBuilder.RenameColumn(
                name: "Address",
                table: "users",
                newName: "address");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "users",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UserRoleId",
                table: "users",
                newName: "user_role_id");

            migrationBuilder.RenameColumn(
                name: "CustomerId",
                table: "users",
                newName: "customer_id");

            migrationBuilder.RenameIndex(
                name: "IX_users_UserRoleId",
                table: "users",
                newName: "ix_users_user_role_id");

            migrationBuilder.RenameIndex(
                name: "IX_users_CustomerId",
                table: "users",
                newName: "ix_users_customer_id");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "user_roles",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "user_roles",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "user_orders",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "StatusId",
                table: "user_orders",
                newName: "status_id");

            migrationBuilder.RenameColumn(
                name: "ShipmentDate",
                table: "user_orders",
                newName: "shipment_date");

            migrationBuilder.RenameColumn(
                name: "OrderNumber",
                table: "user_orders",
                newName: "order_number");

            migrationBuilder.RenameColumn(
                name: "OrderDate",
                table: "user_orders",
                newName: "order_date");

            migrationBuilder.RenameColumn(
                name: "CustomerId",
                table: "user_orders",
                newName: "customer_id");

            migrationBuilder.RenameIndex(
                name: "IX_user_orders_StatusId",
                table: "user_orders",
                newName: "ix_user_orders_status_id");

            migrationBuilder.RenameIndex(
                name: "IX_user_orders_CustomerId",
                table: "user_orders",
                newName: "ix_user_orders_customer_id");

            migrationBuilder.RenameColumn(
                name: "Product_Price",
                table: "products_user_orders",
                newName: "product_price");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "products_user_orders",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UserOrderId",
                table: "products_user_orders",
                newName: "user_order_id");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "products_user_orders",
                newName: "product_id");

            migrationBuilder.RenameColumn(
                name: "ProductCount",
                table: "products_user_orders",
                newName: "product_count");

            migrationBuilder.RenameIndex(
                name: "IX_products_user_orders_UserOrderId",
                table: "products_user_orders",
                newName: "ix_products_user_orders_user_order_id");

            migrationBuilder.RenameIndex(
                name: "IX_products_user_orders_ProductId",
                table: "products_user_orders",
                newName: "ix_products_user_orders_product_id");

            migrationBuilder.RenameColumn(
                name: "Price",
                table: "products",
                newName: "price");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "products",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Code",
                table: "products",
                newName: "code");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "products",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "CategoryId",
                table: "products",
                newName: "category_id");

            migrationBuilder.RenameIndex(
                name: "IX_products_CategoryId",
                table: "products",
                newName: "ix_products_category_id");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "product_categories",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "product_categories",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "order_statuses",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "order_statuses",
                newName: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_users",
                table: "users",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_user_roles",
                table: "user_roles",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_user_orders",
                table: "user_orders",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_products_user_orders",
                table: "products_user_orders",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_products",
                table: "products",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_product_categories",
                table: "product_categories",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_order_statuses",
                table: "order_statuses",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_products_product_category_category_id",
                table: "products",
                column: "category_id",
                principalTable: "product_categories",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_products_user_orders_products_product_id",
                table: "products_user_orders",
                column: "product_id",
                principalTable: "products",
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
                name: "fk_user_orders_order_statuses_status_id",
                table: "user_orders",
                column: "status_id",
                principalTable: "order_statuses",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_user_orders_users_customer_id",
                table: "user_orders",
                column: "customer_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_users_user_roles_user_role_id",
                table: "users",
                column: "user_role_id",
                principalTable: "user_roles",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_users_users_customer_id",
                table: "users",
                column: "customer_id",
                principalTable: "users",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_products_product_category_category_id",
                table: "products");

            migrationBuilder.DropForeignKey(
                name: "fk_products_user_orders_products_product_id",
                table: "products_user_orders");

            migrationBuilder.DropForeignKey(
                name: "fk_products_user_orders_user_order_user_order_id",
                table: "products_user_orders");

            migrationBuilder.DropForeignKey(
                name: "fk_user_orders_order_statuses_status_id",
                table: "user_orders");

            migrationBuilder.DropForeignKey(
                name: "fk_user_orders_users_customer_id",
                table: "user_orders");

            migrationBuilder.DropForeignKey(
                name: "fk_users_user_roles_user_role_id",
                table: "users");

            migrationBuilder.DropForeignKey(
                name: "fk_users_users_customer_id",
                table: "users");

            migrationBuilder.DropPrimaryKey(
                name: "pk_users",
                table: "users");

            migrationBuilder.DropPrimaryKey(
                name: "pk_user_roles",
                table: "user_roles");

            migrationBuilder.DropPrimaryKey(
                name: "pk_user_orders",
                table: "user_orders");

            migrationBuilder.DropPrimaryKey(
                name: "pk_products_user_orders",
                table: "products_user_orders");

            migrationBuilder.DropPrimaryKey(
                name: "pk_products",
                table: "products");

            migrationBuilder.DropPrimaryKey(
                name: "pk_product_categories",
                table: "product_categories");

            migrationBuilder.DropPrimaryKey(
                name: "pk_order_statuses",
                table: "order_statuses");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "users",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "discount",
                table: "users",
                newName: "Discount");

            migrationBuilder.RenameColumn(
                name: "code",
                table: "users",
                newName: "Code");

            migrationBuilder.RenameColumn(
                name: "address",
                table: "users",
                newName: "Address");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "users",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "user_role_id",
                table: "users",
                newName: "UserRoleId");

            migrationBuilder.RenameColumn(
                name: "customer_id",
                table: "users",
                newName: "CustomerId");

            migrationBuilder.RenameIndex(
                name: "ix_users_user_role_id",
                table: "users",
                newName: "IX_users_UserRoleId");

            migrationBuilder.RenameIndex(
                name: "ix_users_customer_id",
                table: "users",
                newName: "IX_users_CustomerId");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "user_roles",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "user_roles",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "user_orders",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "status_id",
                table: "user_orders",
                newName: "StatusId");

            migrationBuilder.RenameColumn(
                name: "shipment_date",
                table: "user_orders",
                newName: "ShipmentDate");

            migrationBuilder.RenameColumn(
                name: "order_number",
                table: "user_orders",
                newName: "OrderNumber");

            migrationBuilder.RenameColumn(
                name: "order_date",
                table: "user_orders",
                newName: "OrderDate");

            migrationBuilder.RenameColumn(
                name: "customer_id",
                table: "user_orders",
                newName: "CustomerId");

            migrationBuilder.RenameIndex(
                name: "ix_user_orders_status_id",
                table: "user_orders",
                newName: "IX_user_orders_StatusId");

            migrationBuilder.RenameIndex(
                name: "ix_user_orders_customer_id",
                table: "user_orders",
                newName: "IX_user_orders_CustomerId");

            migrationBuilder.RenameColumn(
                name: "product_price",
                table: "products_user_orders",
                newName: "Product_Price");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "products_user_orders",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "user_order_id",
                table: "products_user_orders",
                newName: "UserOrderId");

            migrationBuilder.RenameColumn(
                name: "product_id",
                table: "products_user_orders",
                newName: "ProductId");

            migrationBuilder.RenameColumn(
                name: "product_count",
                table: "products_user_orders",
                newName: "ProductCount");

            migrationBuilder.RenameIndex(
                name: "ix_products_user_orders_user_order_id",
                table: "products_user_orders",
                newName: "IX_products_user_orders_UserOrderId");

            migrationBuilder.RenameIndex(
                name: "ix_products_user_orders_product_id",
                table: "products_user_orders",
                newName: "IX_products_user_orders_ProductId");

            migrationBuilder.RenameColumn(
                name: "price",
                table: "products",
                newName: "Price");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "products",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "code",
                table: "products",
                newName: "Code");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "products",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "category_id",
                table: "products",
                newName: "CategoryId");

            migrationBuilder.RenameIndex(
                name: "ix_products_category_id",
                table: "products",
                newName: "IX_products_CategoryId");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "product_categories",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "product_categories",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "order_statuses",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "order_statuses",
                newName: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_users",
                table: "users",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_user_roles",
                table: "user_roles",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_user_orders",
                table: "user_orders",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_products_user_orders",
                table: "products_user_orders",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_products",
                table: "products",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_product_categories",
                table: "product_categories",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_order_statuses",
                table: "order_statuses",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_products_product_categories_CategoryId",
                table: "products",
                column: "CategoryId",
                principalTable: "product_categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_products_user_orders_products_ProductId",
                table: "products_user_orders",
                column: "ProductId",
                principalTable: "products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_products_user_orders_user_orders_UserOrderId",
                table: "products_user_orders",
                column: "UserOrderId",
                principalTable: "user_orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_user_orders_order_statuses_StatusId",
                table: "user_orders",
                column: "StatusId",
                principalTable: "order_statuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_user_orders_users_CustomerId",
                table: "user_orders",
                column: "CustomerId",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_users_user_roles_UserRoleId",
                table: "users",
                column: "UserRoleId",
                principalTable: "user_roles",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_users_users_CustomerId",
                table: "users",
                column: "CustomerId",
                principalTable: "users",
                principalColumn: "Id");
        }
    }
}
