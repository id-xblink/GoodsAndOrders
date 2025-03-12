using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GoodsAndOrders.Migrations
{
    /// <inheritdoc />
    public partial class SeedUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Роли на основе предыдущей миграции
            var roleAdminId = Guid.Parse("843eaeb9-90f4-4ac4-b749-7a7725be3779");
            var roleManagerId = Guid.Parse("3de655ac-561d-4bc8-a5fd-960cc61d6ed2");
            var roleCustomerId = Guid.Parse("25957ef4-7c7f-4ce3-8236-862783449540");

            // Хеши паролей
            var adminHash = "OEt79Jfjf4hENpNvS0jPlw==$pIeIZxCMlpFlBfzpRBO8XYY+AdjqjE3JEZIZVTCm5QA=";
            var managerHash = "9wOfbFdx/IJuo/heEZM3Aw==$CGLHwNDKJE6p0H7c4hR0dCsyoQ9lymNNLRIHpz39atQ=";
            var customerHash = "UnZSziCeV98PrjUYu5zqBQ==$SNKzFLHwG1n3lmtIVNxi+D5tz/s8RZbe+Jzkpsg3B/w=";

            // Данные пользователей
            var users = new (Guid, string, string, string, string, string, int, Guid)[]
            {
                (Guid.NewGuid(), "admin", adminHash, "Админ", "0001-2025", "ул. Административная, д. 1", 0, roleAdminId),
                (Guid.NewGuid(), "manager", managerHash, "Менеджер", "0002-2025", "ул. Управленческая, д. 2", 0, roleManagerId),
                (Guid.NewGuid(), "customer", customerHash, "Заказчик", "0003-2025", "ул. Покупательская, д. 3", 0, roleCustomerId)
            };

            foreach (var (id, login, password, name, code, address, discount, roleId) in users)
            {
                migrationBuilder.InsertData(
                    table: "users",
                    columns: ["id", "login", "password_hash", "name", "code", "address", "discount", "user_role_id"],
                    values: [id, login, password, name, code, address, discount, roleId]
                );
            }
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "users",
                keyColumn: "login",
                keyValues:
                [
                    "admin",
                    "manager",
                    "customer"
                ]
            );
        }
    }
}
