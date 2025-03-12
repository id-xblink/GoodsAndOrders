using GoodsAndOrders.Model.Entities;
using System.ComponentModel.DataAnnotations;

namespace GoodsAndOrders.Model.ModelApi
{
    public class UserRoleModelApi
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
