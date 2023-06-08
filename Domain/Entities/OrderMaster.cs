using Buttler.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Buttler.Domain.Entities
{
    public class OrderMaster
    {
        [Key]
        public int OrderMasterId { get; set; }
        public DateTime? DateOfOrder { get; set; } = DateTime.UtcNow;
        public int? TablesId { get; set; }
        public Tables Tables { get; set; }
        public int? CustomerId { get; set; }
        public Customer Customer { get; set; }

        [Column(TypeName = "nvarchar")]
        [StringLength(450)]
        public string? StaffId { get; set; }
        public decimal? TotalBill { get; set; }
        public OrderStatus.Status OrderStatus { get; set; } = Enums.OrderStatus.Status.pending;
    }
}
