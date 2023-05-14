using Buttler.Application.DTO;
using Buttler.Domain.Data;
using Buttler.Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Buttler_3._0.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ButtlerContext _context;

        public OrdersController(UserManager<AppUser> userManager, ButtlerContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        /// <summary>
        /// Place order of customer
        /// </summary>
        /// <param name="model">Takes the food items, customer id, table number.</param>
        /// <param name="email">staff email.</param>
        /// <returns></returns>
        [Authorize(Roles = "admin,staff")]
        [HttpPost("PlaceOrder")]
        public async Task<IActionResult> PlaceCustomerOrders(PlaceOrderDto model, string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            var table = await _context.Tables.OrderBy(r => r.TablesId)
                .LastOrDefaultAsync(rec => rec.TableNumber == model.TableNumber);
            if (table == null) { return NotFound(new ResultDto<bool>(false, "User and table number are required.")); }
            else if (model != null)
            {
                await _context.OrderMasters.AddAsync(new()
                {
                    CustomerId = table.CustomerId,
                    StaffId = user.Id,
                    TablesId = table.TablesId,
                    OrderStatus = (int)model.OrderStatus,
                    DateOfOrder = model.DateOfOrder,
                });

                await _context.SaveChangesAsync();

                var ordMst = await _context.OrderMasters
                    .FirstOrDefaultAsync(r => r.CustomerId == table.CustomerId);

                if (ordMst != null)
                {
                    decimal? bill = 0.0M;
                    foreach (var foodItem in model.Foods)
                    {
                        await _context.OrderItems.AddAsync(new()
                        {
                            FoodsId = foodItem.FoodId,
                            Quantity = foodItem.Quantity,
                            OrderMasterId = ordMst.OrderMasterId
                        });
                        bill += (foodItem.Price * foodItem.Quantity);
                    }
                    ordMst.TotalBill = bill;
                    await _context.SaveChangesAsync();
                }
                return Ok(new ResultDto<bool>(true, "Your order is successfully stored."));
            }
            return BadRequest(new ResultDto<bool>(false, "Something went wrong."));
        }
    }
}
