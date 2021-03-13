using Api.VM;
using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Service.Contract;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [ApiVersionNeutral]
    [Route("api/Account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly ISmsSender _smsSender;

        public AccountController(UserManager<User> userManager,
            IEmailSender emailSender,
            ISmsSender smsSender)
        {
            _userManager = userManager;
            _emailSender = emailSender;
            _smsSender = smsSender;
        }

        [HttpPost("Register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] AuthentificationVM model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                return BadRequest(errors);
            }

            var user = new User
            {
                UserName = model.UserName,
                FirstName = model.FirstName,
                LastName = model.LastName,
                BirthDate = model.BirthDate,
                Email = model.Email
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                return BadRequest(errors);
            }

            //Confirmation email
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var encoder = UrlEncoder.Create();
            var callbackUrl = $"https://localhost:44334/api/Account/ConfirmEmail?userId={encoder.Encode(user.Id)}&code={encoder.Encode(code)}&redirect={encoder.Encode(model.Redirect)}";
            await _emailSender.SendEmailAsync(model.Email, "Confirm your account",
                "Please confirm your account by clicking this link:  <a href=\"" + callbackUrl + "\">link</a>");

            return Ok();
        }

        [HttpGet("ConfirmEmail")]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail([FromQuery] string userId, [FromQuery] string code, [FromQuery] string redirect)
        {
            //string userId = HttpContext.Request.Query.First(q => q.Key == "userId").Value;
            //string code = HttpContext.Request.Query.First(q => q.Key == "code").Value;
            //string redirect = HttpContext.Request.Query.First(q => q.Key == "redirect").Value;
            if (userId == null || code == null || redirect == null)
            {
                return BadRequest(
                    "userId and code must be full"
                    );
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null)
            {
                return BadRequest();
            }
            var result = await _userManager.ConfirmEmailAsync(user, code);

            return result.Succeeded ? string.IsNullOrEmpty(redirect) ? Ok() : Redirect(redirect) : BadRequest();
        }

        [HttpPost("ForgotPassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                return BadRequest(errors);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user is null || !(await _userManager.IsEmailConfirmedAsync(user)))
            {
                // Don't reveal that the user does not exist or is not confirmed
                return Ok();
            }

            // Send an email with this link
            var code = await _userManager.GeneratePasswordResetTokenAsync(user);

            var callbackUrl = "https://yourfrontwebsiteResetpage?userId="+ user.Id +"code="+code;
            await _emailSender.SendEmailAsync(model.Email, "Reset Password",
                "Please reset your password by clicking this link:  <a href=\"" + callbackUrl + "\">link</a>");
#if DEBUG
            return Ok(new { code, user.Id });
#endif
#if !DEBUG
            return Ok();
#endif
        }

        [HttpPost("ResetPassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                return BadRequest(errors);
            }
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user is null)
            {
                // Don't reveal that the user does not exist
                if (model.Redirect == null)
                {
                    Ok();
                }
                return Redirect(model.Redirect);
            }
            var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                return BadRequest(errors);
            }
            if (model.Redirect == null)
            {
                return Ok();
            }
            return Redirect(model.Redirect);
        }
    }
}
