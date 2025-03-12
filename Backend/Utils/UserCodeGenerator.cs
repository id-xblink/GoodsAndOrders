namespace GoodsAndOrders.Utils
{
    public class UserCodeGenerator
    {
        public static string GenerateUserCode(List<string> existingCodes)
        {
            string year = DateTime.UtcNow.Year.ToString();

            var lastCode = existingCodes
                 .Where(code => code.EndsWith(year))
                 .AsEnumerable()
                 .Select(c => int.TryParse(c.Split('-')[0], out int num) ? num : 0)
                 .DefaultIfEmpty(0)
                 .Max();

            return $"{(lastCode + 1):D4}-{year}";
        }
    }
}