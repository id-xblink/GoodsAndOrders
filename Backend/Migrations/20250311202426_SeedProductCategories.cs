using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GoodsAndOrders.Migrations
{
    /// <inheritdoc />
    public partial class SeedProductCategories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var categories = new (Guid, string)[]
            {
                (Guid.NewGuid(), "Одежда и аксессуары"),
                (Guid.NewGuid(), "Электроника и техника"),
                (Guid.NewGuid(), "Дом и интерьер"),
                (Guid.NewGuid(), "Бытовая химия и косметика"),
                (Guid.NewGuid(), "Продукты питания"),
                (Guid.NewGuid(), "Детские товары"),
                (Guid.NewGuid(), "Автотовары"),
                (Guid.NewGuid(), "Спорт и отдых"),
                (Guid.NewGuid(), "Зоотовары"),
                (Guid.NewGuid(), "Канцелярия и книги")
            };

            foreach (var (id, name) in categories)
            {
                migrationBuilder.InsertData(
                    table: "product_categories",
                    columns: ["id", "name"],
                    values: [id, name]
                );
            }
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
            table: "product_categories",
            keyColumn: "name",
            keyValues:
            [
                "Одежда и аксессуары",
                "Электроника и техника",
                "Дом и интерьер",
                "Бытовая химия и косметика",
                "Продукты питания",
                "Детские товары",
                "Автотовары",
                "Спорт и отдых",
                "Зоотовары",
                "Канцелярия и книги"
            ]);
        }
    }
}
