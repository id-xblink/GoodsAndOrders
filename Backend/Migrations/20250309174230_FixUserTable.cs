using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GoodsAndOrders.Migrations
{
    /// <inheritdoc />
    public partial class FixUserTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_users_users_customer_id",
                table: "users");

            migrationBuilder.DropIndex(
                name: "ix_users_customer_id",
                table: "users");

            migrationBuilder.DropColumn(
                name: "customer_id",
                table: "users");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "customer_id",
                table: "users",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_users_customer_id",
                table: "users",
                column: "customer_id");

            migrationBuilder.AddForeignKey(
                name: "fk_users_users_customer_id",
                table: "users",
                column: "customer_id",
                principalTable: "users",
                principalColumn: "id");
        }
    }
}
