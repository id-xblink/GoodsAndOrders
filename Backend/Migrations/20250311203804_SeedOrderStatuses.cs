using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GoodsAndOrders.Migrations
{
    /// <inheritdoc />
    public partial class SeedOrderStatuses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var statuses = new (Guid, string)[]
            {
                (Guid.NewGuid(), "Новый"),
                (Guid.NewGuid(), "Выполняется"),
                (Guid.NewGuid(), "Выполнен")
            };

            foreach (var (id, name) in statuses)
            {
                migrationBuilder.InsertData(
                    table: "order_statuses",
                    columns: ["id", "name"],
                    values: [id, name]
                );
            }
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "order_statuses",
                keyColumn: "name",
                keyValues:
                [
                    "Новый",
                    "Выполняется",
                    "Выполнен"
                ]
            );
        }
    }
}
