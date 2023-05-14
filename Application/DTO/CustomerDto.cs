using System.Text.Json.Serialization;

namespace Buttler.Application.DTO
{
    public class CustomerDto
    {
        [JsonIgnore]
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerGender { get; set; }
        public string PhoneNumber { get; set; }
    }

    public class TablesDto
    {
        [JsonIgnore]
        public int TableId { get; set; }
        public int TableNumber { get; set; }
        public int CustomerId { get; set; }
    }
}
