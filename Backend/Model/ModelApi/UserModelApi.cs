namespace GoodsAndOrders.Model.ModelApi
{
    public class UserModelApi
    {
        public Guid Id { get; set; }
        public string Login { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public int Discount { get; set; }
        public Guid UserRoleId { get; set; }
    }
}
