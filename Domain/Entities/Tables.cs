namespace Buttler.Domain.Entities
{
    public class Tables
    {
        public int TablesId { get; set; }
        public int TableNumber { get; set; }
        public int? CustomerId { get; set; }
        public Customer Customer { get; set; }
    }
}
