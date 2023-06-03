namespace Buttler.Application.DTO
{
    public class FoodsDto
    {
        public int FoodId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string FoodImg { get; set; }
        public decimal? Price { get; set; }
        public int Quantity { get; set; }
    }

    public class FoodId
    {
        public int FoodItemId { get; set; }
    }
}
