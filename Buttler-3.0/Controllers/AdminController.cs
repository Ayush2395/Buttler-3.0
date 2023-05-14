using Buttler.Application.Common.Query.OrderBill;
using Buttler.Application.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Buttler_3._0.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "admin")]
    public class AdminController : BaseController
    {
        [HttpGet("BillsForAdminDisplay")]
        public async Task<IActionResult> GetAllCustomerBill()
        {
            var result = await Mediator.Send(new OrderBillQuery());
            return result != null ? Ok(new ResultDto<List<BillingDto>>(true, result)) : BadRequest(new ResultDto<bool>(false, "Something went wrong."));
        }
    }
}
