namespace GoodsAndOrders.Model.ModelApi
{
    public class OrderResponseModelApi
    {
        public Guid Id { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime? ShipmentDate { get; set; }
        public int OrderNumber { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}
