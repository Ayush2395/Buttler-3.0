using Buttler.Application.Common.Command.Admin;
using Buttler.Application.Common.Query.OrderBill;
using Buttler.Application.DTO;
using Buttler.Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Buttler_3._0.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "admin")]
    public class AdminController : BaseController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpGet("BillsForAdminDisplay")]
        public async Task<IActionResult> GetAllCustomerBill()
        {
            var result = await Mediator.Send(new OrderBillQuery());
            return result != null ? Ok(new ResultDto<List<BillingDto>>(true, result)) : BadRequest(new ResultDto<bool>(false, "Something went wrong."));
        }

        /// <summary>
        /// Register new user with credentials
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Give user credentials as the model given below.</returns>
        [HttpPost("RegisterStaff")]
        public async Task<IActionResult> RegisterNewUser([FromBody] RegisterDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            var passwordValidator = _userManager.PasswordValidators;
            List<string> errors = new();
            foreach (var password in passwordValidator)
            {
                var result = await password.ValidateAsync(_userManager, null!, model.Password);
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        errors.Add(error.Description);
                    }
                    return BadRequest(new ResultDto<List<string>>(false, errors));
                }
            }
            if (user != null)
            {
                return BadRequest(new ResultDto<bool>(false, "User already exist."));
            }
            if (user == null)
            {
                var staffRole = await _roleManager.FindByNameAsync("staff");
                var userDetails = new AppUser
                {
                    UserName = model.UserName,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    CreatedAt = DateTime.UtcNow,
                    Gender = model.Gender,
                    PhoneNumber = model.PhoneNumber,
                };

                var newUser = await _userManager.CreateAsync(userDetails, model.Password);
                if (newUser.Succeeded)
                {
                    await _userManager.AddToRoleAsync(userDetails, staffRole.Name);
                    return Ok(new ResultDto<bool>(true, "Your account is registered."));
                }

                return BadRequest(new ResultDto<IEnumerable<IdentityError>>(false, newUser.Errors));

            }
            return BadRequest();
        }

        [HttpPatch("UpdateOrderStatus/{OrderStatus}/{OrderMasterId}")]
        public async Task<IActionResult> UpdateOrderStatus([FromRoute] int OrderStatus, [FromRoute] int OrderMasterId)
        {
            return Ok(await Mediator.Send(new UpdateOrderStatusCommand { OrderMasterId = OrderMasterId, OrderStatus = OrderStatus }));
        }

        [HttpDelete("DeleteStaffData/{email}")]
        public async Task<IActionResult> DeleteStaffById([FromRoute] string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                await _userManager.DeleteAsync(user);
                return Ok(new ResultDto<bool>(true, true, "Staff successfully removed."));
            }
            return BadRequest(new ResultDto<bool>(false, false, "Something went wrong."));
        }

        [HttpGet("GetAllStaffs")]
        public async Task<IActionResult> GetAllStaff()
        {
            var getStaffRole = await _roleManager.FindByNameAsync("staff");
            var staffUsers = _userManager
                            .GetUsersInRoleAsync(getStaffRole.Name).Result
                            .Select(r => new UserDetailsDto
                            {
                                Age = r.Age,
                                Email = r.Email,
                                FirstName = r.FirstName,
                                LastName = r.LastName,
                                Gender = r.Gender,
                                JoiningDate = r.CreatedAt,
                                UserName = r.UserName,
                            });
            return Ok(new ResultDto<IEnumerable<UserDetailsDto>>(true,staffUsers));
        }
    }
}
