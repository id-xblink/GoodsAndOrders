namespace GoodsAndOrders.Model.ModelApi
{
    public class CreateOrderModelApi
    {
        public Guid CustomerId { get; set; }
        // Товары в заказе
        public List<ProductOrderItem> Items { get; set; } = new();
    }

    public class ProductOrderItem
    {
        public Guid ProductId { get; set; }
        // Количество
        public int Quantity { get; set; }
    }
}
