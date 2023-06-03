using Buttler.Domain.Enums;

namespace Buttler.Application.DTO
{
    public class BillingDto
    {
        public string CustomerName { get; set; }
        public string CustomerPhoneNumber { get; set; }
        public int OrderStatus { get; set; }
        public decimal? Bill { get; set; }
        public DateTime? DateOfOrder { get; set; }
    }
}
