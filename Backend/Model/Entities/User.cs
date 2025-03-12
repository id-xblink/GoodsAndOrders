using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GoodsAndOrders.Model.Entities
{
    public class User
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(20)]
        public string Login { get; set; } = string.Empty;

        [Required]
        [MaxLength(255)]
        public string PasswordHash { get; set; } = string.Empty;

        [Required]
        [MaxLength(30)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(9)]
        public string Code { get; set; } = string.Empty;

        [Required]
        [MaxLength(200)]
        public string Address { get; set; } = string.Empty;

        [Required]
        public int Discount { get; set; }
        [Required]
        public Guid UserRoleId { get; set; }


        [ForeignKey(nameof(UserRoleId))]
        public UserRole? UserRole { get; set; }

        public ICollection<UserOrder> UserOrders { get; set; } = new List<UserOrder>();
    }
}
