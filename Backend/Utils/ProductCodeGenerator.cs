namespace GoodsAndOrders.Utils
{
    public class ProductCodeGenerator
    {
        private static readonly Random _random = new Random();

        public static string GenerateUniqueCode(IEnumerable<string> existingCodes)
        {
            string newCode;
            do
            {
                newCode = GenerateRandomCode();
            } while (existingCodes.Contains(newCode)); // Проверка уникальности кода

            return newCode;
        }

        private static string GenerateRandomCode()
        {
            int part1 = Random.Shared.Next(0, 100);
            int part2 = Random.Shared.Next(0, 10000);
            string part3 = RandomLetters(2);
            int part4 = Random.Shared.Next(0, 100);

            return $"{part1:D2}-{part2:D4}-{part3}{part4:D2}";
        }

        private static string RandomLetters(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[Random.Shared.Next(s.Length)]).ToArray());
        }
    }
}
