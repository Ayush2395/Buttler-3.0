using Buttler.Application.DTO;
using Buttler.Domain.Data;

namespace Buttler.Application.Repositories
{
    public class FoodRepo : IFoodRepo
    {
        private readonly ButtlerContext _context;

        public FoodRepo(ButtlerContext context)
        {
            _context = context;
        }

        public List<FoodsDto> FoodItems()
        {
            return _context.Foods.Select(rec => new FoodsDto()
            {
                FoodId = rec.FoodsId,
                Title = rec.Title,
                Description = rec.Description,
                FoodImg = rec.FoodImg,
                Price = rec.Price,
            }).ToList();
        }
    }

    public interface IFoodRepo
    {
        List<FoodsDto> FoodItems();
    }
}
