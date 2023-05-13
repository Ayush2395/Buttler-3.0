using Buttler.Domain.Enums;
using System.ComponentModel.DataAnnotations;

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
        public int? StaffId { get; set; }
        public decimal? TotalBill { get; set; }
        public OrderStatus.Status OrderStatus { get; set; } = Enums.OrderStatus.Status.pending;
    }
}
