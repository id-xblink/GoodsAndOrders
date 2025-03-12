using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GoodsAndOrders.Model.Entities
{
    public class UserOrder
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [Column(TypeName = "timestamp with time zone")]
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        [Column(TypeName = "timestamp with time zone")]
        public DateTime? ShipmentDate { get; set; }

        [Required]
        public int OrderNumber { get; set; }

        [Required]
        public Guid CustomerId { get; set; }

        [Required]
        public Guid StatusId { get; set; }


        [ForeignKey(nameof(CustomerId))]
        public User? Customer { get; set; }

        [ForeignKey(nameof(StatusId))]
        public OrderStatus? OrderStatus { get; set; }


        public ICollection<ProductUserOrder> OrderElements { get; set; } = new List<ProductUserOrder>();
    }
}
