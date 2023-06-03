using Buttler.Application.Common.Commanda.Customer;
using Buttler.Application.Common.Query.Food;
using Buttler.Application.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Buttler_3._0.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    [Authorize(Roles = "admin,staff")]
    public class FoodController : BaseController
    {
        /// <summary>
        /// Get all food Items list after login.
        /// </summary>
        /// <returns></returns>
        [HttpGet("AllFoodItems")]
        public async Task<IActionResult> GetFoodItems()
        {
            var result = await Mediator.Send(new FoodListQuery());
            return result != null ? Ok(new ResultDto<List<FoodsDto>>(true, result)) : NotFound(new ResultDto<bool>(false, "No food items found."));
        }

        /// <summary>
        /// Add customer details for billing purpose
        /// </summary>
        /// <param name="customer">Take customer details.</param>
        /// <returns></returns>
        [HttpPost("AddCustomerDetails")]
        public async Task<IActionResult> AddCustomerDetails(CustomerDto customer)
        {
            var result = await Mediator.Send(new CustomerDetailCommand { Customer = customer });
            return Ok(new { customerId = result });
        }

        /// <summary>
        /// Book table for customer.
        /// </summary>
        /// <param name="table">Take table number and pass to the</param>
        /// <returns></returns>
        [HttpPost("BookTableForCustomer")]
        public async Task<IActionResult> BookTableForCustomer(TablesDto table)
        {
            var result = await Mediator.Send(new CustomerTableCommand { Table = table });
            return result != null ? Ok(result) : BadRequest(new ResultDto<bool>(false, "Fill the table number."));
        }
    }
}
