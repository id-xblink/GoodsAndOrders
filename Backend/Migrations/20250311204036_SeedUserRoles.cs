using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GoodsAndOrders.Migrations
{
    /// <inheritdoc />
    public partial class SeedUserRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var roles = new (Guid, string)[]
            {
                (Guid.Parse("843eaeb9-90f4-4ac4-b749-7a7725be3779"), "Админ"),
                (Guid.Parse("3de655ac-561d-4bc8-a5fd-960cc61d6ed2"), "Менеджер"),
                (Guid.Parse("25957ef4-7c7f-4ce3-8236-862783449540"), "Заказчик")
            };

            foreach (var (id, name) in roles)
            {
                migrationBuilder.InsertData(
                    table: "user_roles",
                    columns: ["id", "name"],
                    values: [id, name]
                );
            }
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "user_roles",
                keyColumn: "name",
                keyValues:
                [
                    "Админ",
                    "Менеджер",
                    "Заказчик"
                ]
            );
        }
    }
}
