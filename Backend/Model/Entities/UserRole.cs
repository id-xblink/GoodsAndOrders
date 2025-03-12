using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GoodsAndOrders.Model.Entities
{
    public class UserRole
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(20)]
        public string Name { get; set; } = string.Empty;


        public ICollection<User> Users { get; set; } = new List<User>();
    }
}
