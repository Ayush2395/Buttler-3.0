using System.ComponentModel.DataAnnotations;

namespace Buttler.Domain.Entities
{
    public class Foods
    {
        [Key]
        public int FoodsId { get; set; }

        [StringLength(85)]
        public string? Title { get; set; }

        [StringLength(256)]
        public string? Description { get; set; }
        public string? FoodImg { get; set; }
    }
}
