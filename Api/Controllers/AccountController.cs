using Api.VM;
using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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

        //POST /api/Account/Register
        /// <summary>
        /// Register a new account with the given model
        /// </summary>
        /// <param name="model">The values of the ViewModel who describe the account</param>
        /// <returns>Task<IActionResult> with StatusCodes 200 OK OR 400 BAD REQUEST</returns>
        [HttpPost("Register")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register([FromBody] AuthentificationViewModel model)
        {
            //verify the VM have all necessary info 
            if (!ModelState.IsValid)
            {
                //return 400 with the necessary info who miss
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

            //try to create the new user
            var result = await _userManager.CreateAsync(user, model.Password);
            //if the user is not create
            if (!result.Succeeded)
            {
                //return 400 with the errors
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                return BadRequest(errors);
            }

            //Send a email to confirm his mail adress
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var encoder = UrlEncoder.Create();
            var callbackUrl = $"https://localhost:44334/api/Account/ConfirmEmail?userId={encoder.Encode(user.Id)}&code={encoder.Encode(code)}&redirect={encoder.Encode(model.Redirect)}";
            await _emailSender.SendEmailAsync(model.Email, "Confirm your account",
                "Please confirm your account by clicking this link:  <a href=\"" + callbackUrl + "\">link</a>");

            return Ok();
        }

        //POST /api/Account/ConfirmEmail
        /// <summary>
        /// Confirm the email with the given userId, code and redirect with the given redirectUrl
        /// </summary>
        /// <param name="userId">The values of the userId</param>
        /// <param name="code">The values of the code</param>
        /// <param name="redirect">The values of the redirect url</param>
        /// <returns>Task<IActionResult> with StatusCodes 302 FOUND OR 400 BAD REQUEST</returns>
        [HttpGet("ConfirmEmail")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status302Found)]
        public async Task<IActionResult> ConfirmEmail([FromQuery] string userId, [FromQuery] string code, [FromQuery] string redirect)
        {
            //Verify that all info is send
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(code) || string.IsNullOrEmpty(redirect))
            {
                return BadRequest(
                    "userId, code and redirect must be full"
                    );
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null)
            {
                return BadRequest();
            }

            //confirm the email to the given user and given code
            var result = await _userManager.ConfirmEmailAsync(user, code);

            return result.Succeeded ? Redirect(redirect) : BadRequest();
        }

        //POST /api/Account/ForgotPassword
        /// <summary>
        /// Send a email for change the password if the user have forgot it with the given model
        /// </summary>
        /// <param name="model">The values of the ViewModel</param>
        /// <returns>Task<IActionResult> with StatusCodes 200 OK OR 400 BAD REQUEST</returns>
        [HttpPost("ForgotPassword")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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

        //POST /api/Account/ResetPassword
        /// <summary>
        /// Reset the password with the given model
        /// </summary>
        /// <param name="model">The values of the ViewModel who descibe the user parameter for reset password</param>
        /// <returns>Task<IActionResult> with StatusCodes 200 OK OR 400 BAD REQUEST OR 302 FOUND</returns>
        [HttpPost("ResetPassword")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status302Found)]
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
