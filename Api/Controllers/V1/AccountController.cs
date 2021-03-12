using Api.VM;
using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Service.Contract;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Controllers.V1
{
    [ApiVersion("1.0")]
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
        public async Task<IActionResult> Register(AuthentificationVM userVM)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                return BadRequest(errors);
            }

            var user = new User
            {
                UserName = userVM.UserName,
                FirstName = userVM.FirstName,
                LastName = userVM.LastName,
                BirthDate = userVM.BirthDate,
                Email = userVM.Email
            };

            var result = await _userManager.CreateAsync(user, userVM.Password);
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
            var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code, redirect = userVM.Redirect }, protocol: HttpContext.Request.Scheme);
            await _emailSender.SendEmailAsync(userVM.Email, "Confirm your account",
                "Please confirm your account by clicking this link:  <a href=\"" + callbackUrl + "\">link</a>");

            return Ok();
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string code, string redirect)
        {
            if (userId is null || code is null)
            {
                return BadRequest();
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
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
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
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
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
