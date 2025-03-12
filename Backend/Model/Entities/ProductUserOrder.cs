using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GoodsAndOrders.Model.Entities
{
    public class ProductUserOrder
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public Guid ProductId { get; set; }

        [Required]
        public Guid UserOrderId { get; set; }

        [Required]
        public int ProductCount { get; set; }

        [Required]
        [Column(TypeName = "NUMERIC(18,2)")]
        public decimal ProductPrice { get; set; }


        [ForeignKey(nameof(ProductId))]
        public Product? Product { get; set; }

        [ForeignKey(nameof(UserOrderId))]
        public UserOrder? UserOrder { get; set; }
    }
}
