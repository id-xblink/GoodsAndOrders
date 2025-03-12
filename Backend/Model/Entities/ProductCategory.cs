using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GoodsAndOrders.Model.Entities
{
    public class ProductCategory
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(30)]
        public string Name { get; set; } = string.Empty;


        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
