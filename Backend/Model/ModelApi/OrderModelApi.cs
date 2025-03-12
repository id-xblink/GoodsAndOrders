namespace GoodsAndOrders.Model.ModelApi
{
    public class OrderModelApi
    {
        public Guid Id { get; set; }

        public DateTime OrderDate { get; set; }

        public DateTime? ShipmentDate { get; set; }

        public int OrderNumber { get; set; }

        public Guid CustomerId { get; set; }

        public Guid StatusId { get; set; }

        // Список товаров в заказе
        public List<OrderProductItem> Items { get; set; } = new();
    }

    public class OrderProductItem
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
