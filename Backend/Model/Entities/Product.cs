using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GoodsAndOrders.Model.Entities
{
    public class Product
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(20)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(12)]
        public string Code { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "NUMERIC(18,2)")]
        public decimal Price { get; set; }

        [Required]
        public Guid CategoryId { get; set; }


        [ForeignKey(nameof(CategoryId))]
        public ProductCategory Category { get; set; } = null!;


        public ICollection<ProductUserOrder> OrderElements { get; set; } = new List<ProductUserOrder>();
    }
}
