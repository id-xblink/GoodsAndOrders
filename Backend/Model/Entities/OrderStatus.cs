using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GoodsAndOrders.Model.Entities
{
    public class OrderStatus
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(20)]
        public string Name { get; set; } = string.Empty;

        public ICollection<UserOrder> UserOrders { get; set; } = new List<UserOrder>();
    }
}
