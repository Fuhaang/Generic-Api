using Api.VM;
using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Controllers.V1
{
    [AllowAnonymous]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<User> _userManager;

        public AccountController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost("api/v1/Account/Register")]
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

            return Ok();
        }
    }
}
