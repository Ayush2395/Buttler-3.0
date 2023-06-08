using Buttler.Application.DTO;
using Buttler.Infrastructure.Identity;
using Buttler_3._0.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Web;

namespace Buttler_3._0.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IJwtTokenService _jwtTokenService;

        public AuthController(UserManager<AppUser> userManager, IJwtTokenService jwtTokenService, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _jwtTokenService = jwtTokenService;
            _roleManager = roleManager;
        }

        /// <summary>
        /// Login user
        /// </summary>
        /// <param name="login">Enter the registered user credentials</param>
        /// <returns>It'll return an object with JWT Token.</returns>
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDto login)
        {
            var user = await _userManager.FindByEmailAsync(login.Email);
            if (user == null) { return NotFound(new ResultDto<bool>(false, "User not found.")); }
            if (user != null && await _userManager.CheckPasswordAsync(user, login.Password))
            {
                var result = _jwtTokenService.CreateJwtSecurityTokenHandler(login);
                var tokenGen = new JwtSecurityTokenHandler().WriteToken(result);
                var token = new JwtTokenModel
                {
                    AccessKey = tokenGen,
                    Email = user.Email,
                    UserName = user.UserName,
                    Roles = _userManager.GetRolesAsync(user).Result,
                    IsEmailConfirmed = user.EmailConfirmed,
                    IsTwoFactorAuthentication = user.TwoFactorEnabled,
                    DurationInMinutes = result.ValidTo
                };
                return Ok(new ResultDto<JwtTokenModel>(true, token));
            }
            return BadRequest(new ResultDto<bool>(false, "Wrong password."));
        }

        /// <summary>
        /// Send the email verification code, but now returning only url string
        /// </summary>
        /// <param name="email">Enter user's email.</param>
        /// <returns></returns>
        [HttpPost("SendResetPasswordEmail")]
        public async Task<IActionResult> SendResetPasswordEmail(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return NotFound(new ResultDto<bool>(false, "User not found with this email."));
            }
            if (user != null)
            {
                var verificationCode = await _userManager.GeneratePasswordResetTokenAsync(user);
                var encodeCode = HttpUtility.UrlEncode(verificationCode);
                var callbacURL = $"http://localhost:4200/reset-password?email={user.Email}&code={encodeCode}";
                return Ok(new ResultDto<string>(true, callbacURL));
            }
            return BadRequest(new ResultDto<bool>(false, "Something webt wrong."));
        }

        /// <summary>
        /// Reset users password.
        /// </summary>
        /// <param name="model">It Takes users credentials like email and password.</param>
        /// <param name="code">verification code which is sent on email.</param>
        /// <returns>It will reset password after taking email, password and verification code</returns>
        /// <remarks>
        /// Sample Request : {
        ///     "email" : "ayushkrishanmandal@gmail.com",
        ///     "password" : "Piyushmandal@005355,
        ///     "code" : "xyz"
        /// }
        /// </remarks>
        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] LoginDto model, string code)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return NotFound(new ResultDto<bool>(false, "User not found."));
            }
            if (user != null)
            {
                var passwordValidation = _userManager.PasswordValidators;
                List<string> errors = new();
                foreach (var password in passwordValidation)
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
                var confirmation = await _userManager.ResetPasswordAsync(user, code, model.Password);
                if (confirmation.Succeeded)
                {
                    return Ok(new ResultDto<bool>(true, "Your password is reset."));
                }
                return BadRequest(new ResultDto<bool>(false, "Invalid Token or token is expired."));
            }
            return BadRequest(new ResultDto<bool>(false, "Something went wrong."));
        }

        /// <summary>
        /// User can change the password, when he/she is logged in.
        /// </summary>
        /// <param name="model">user credentials with new password</param>
        /// <param name="currentPassword">Enter the current password for validation.</param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("ChangePassword/{currentPassword}")]
        public async Task<IActionResult> ChangePassword(LoginDto model, [FromRoute] string currentPassword)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return NotFound(new ResultDto<bool>(false, "User not found."));
            }
            if (user != null)
            {
                var passwordValidation = _userManager.PasswordValidators;
                var errors = new List<string>();
                foreach (var password in passwordValidation)
                {
                    var passValidator = await password.ValidateAsync(_userManager, null!, model.Password);
                    if (!passValidator.Succeeded)
                    {
                        foreach (var err in passValidator.Errors)
                        {
                            errors.Add(err.Description);
                        }
                        return BadRequest(new ResultDto<List<string>>(false, errors));
                    }
                }
                var result = await _userManager.ChangePasswordAsync(user, currentPassword, model.Password);
                if (result.Succeeded)
                {
                    return Ok(new ResultDto<bool>(true, "Your password is changed."));
                }
            }
            return BadRequest(new ResultDto<bool>(false, "You exceed the limit of change password, try next time."));
        }

        /// <summary>
        /// Send email confirmation mail.
        /// </summary>
        /// <param name="email">User email.</param>
        /// <returns></returns>
        [HttpPost("SendEmailConfirmationMail")]
        public async Task<IActionResult> SendEmailConfirmationMail(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) { return NotFound(new ResultDto<bool>(false, "User not found.")); }
            if (user != null)
            {
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var encode = HttpUtility.UrlEncode(code);
                var callbackURL = $"http://localhost:4200/verification?email={user.Email}&code={encode}";
                return Ok(new ResultDto<string>(false, callbackURL));
            }
            return BadRequest(new ResultDto<bool>(false, "Something went wrong."));
        }

        /// <summary>
        /// Email Confirmation
        /// </summary>
        /// <param name="email">Takes user's email</param>
        /// <param name="code">Verification code</param>
        /// <returns></returns>
        [HttpPost("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string email, string code)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) { return NotFound(new ResultDto<bool>(false, "User not found.")); }
            if (user != null)
            {
                var result = await _userManager.ConfirmEmailAsync(user, code);
                if (result.Succeeded)
                {
                    return Ok(new ResultDto<bool>(true, "Email is confirmed."));
                }
                return BadRequest(new ResultDto<bool>(false, "Invalid token or token is expired."));
            }
            return BadRequest(new ResultDto<bool>(false, "Somthing went wrong."));
        }

        [Authorize(Roles = "admin,staff")]
        [HttpGet("UserDetails")]
        public async Task<IActionResult> GetUserDetails(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            var role = await _userManager.GetRolesAsync(user);
            if (role == null) { return NotFound("User not found."); }
            return Ok(new ResultDto<object>(true, new { user.UserName, user.FirstName, user.LastName, user.PhoneNumber, user.Email, user.EmailConfirmed, user.Age, user.Gender, user.ProfilePic, user.CreatedAt }, null!));
        }

        [HttpPost("UpdateProfile")]
        public async Task<IActionResult> UpdateProfile([FromBody] UserDetailsDto user, string email)
        {
            var userData = await _userManager.FindByEmailAsync(email);
            if (userData != null)
            {
                userData.FirstName = user.FirstName;
                userData.LastName = user.LastName;
                userData.PhoneNumber = user.PhoneNumber;
                user.Age = user.Age;
                userData.Email = user.Email;

                await _userManager.UpdateAsync(userData);

                return Ok(new ResultDto<bool>(true, true, "Profile is updated."));
            }
            return BadRequest(new ResultDto<bool>(false, false, "Profile is updated."));
        }
    }
}
