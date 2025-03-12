namespace GoodsAndOrders.Model.ModelApi
{
    public class UpdateProductModelApi
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Code { get; set; } = string.Empty;
        public Guid CategoryId { get; set; }
    }
}
