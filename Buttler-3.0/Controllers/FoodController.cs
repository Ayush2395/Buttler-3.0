using Buttler.Application.Common.Query.Food;
using Buttler.Application.DTO;
using Microsoft.AspNetCore.Mvc;

namespace Buttler_3._0.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class FoodController : BaseController
    {
        [HttpGet("AllFoodItems")]
        public async Task<IActionResult> GetFoodItems()
        {
            var result = await Mediator.Send(new FoodListQuery());
            return result != null ? Ok(new ResultDto<List<FoodsDto>>(true, result)) : NotFound(new ResultDto<bool>(false, "No food items found."));
        }
    }
}
