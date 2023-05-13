namespace Buttler.Domain.Entities
{
    public class OrderItems
    {
        public int OrderItemsId { get; set; }

        public int? OrderMasterId { get; set; }
        public OrderMaster OrderMaster { get; set; }

        public int? FoodsId { get; set; }
        public Foods? Foods { get; set; }

        public int? Quantity { get; set; }
    }
}
