using Buttler.Domain.Enums;
using System.Text.Json.Serialization;

namespace Buttler.Application.DTO
{
    public class PlaceOrderDto : OrderItemsDto
    {
        public int CustomerId { get; set; }
        public int TableNumber { get; set; }
        [JsonIgnore]
        public int StaffId { get; set; }

        [JsonIgnore]
        public OrderStatus.Status OrderStatus { get; set; } = Domain.Enums.OrderStatus.Status.pending;

        [JsonIgnore]
        public decimal Bill { get; set; }

        [JsonIgnore]
        public DateTime DateOfOrder { get; set; } = DateTime.Now;
    }

    public class OrderItemsDto
    {
        public List<FoodsDto> Foods { get; set; }

        [JsonIgnore]
        public int Quantity { get; set; }
    }
}
