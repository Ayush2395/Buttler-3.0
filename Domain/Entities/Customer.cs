using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Buttler.Domain.Entities
{
    public class Customer
    {
        [Key]
        public int CustomerId { get; set; }

        [Column(TypeName = "varchar")]
        [StringLength(20)]
        public string? CustomerName { get; set; }

        [Column(TypeName = "varchar")]
        [StringLength(10)]
        public string? PhoneNumber { get; set; }

        [Column(TypeName = "varchar")]
        [StringLength(10)]
        public string? Gender { get; set; }
    }
}
