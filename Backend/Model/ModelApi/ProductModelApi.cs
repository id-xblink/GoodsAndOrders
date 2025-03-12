namespace GoodsAndOrders.Model.ModelApi
{
    public class ProductModelApi
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Code { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;
    }
}
