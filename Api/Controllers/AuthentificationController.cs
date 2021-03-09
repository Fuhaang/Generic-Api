using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Api.VM;
using Entities;
using EntitiesContext;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using UnitOfWork.Contract;

namespace Api.Controllers
{
    [AllowAnonymous]
    public class AuthentificationController : ControllerBase
    {

        private readonly IUnitOfWork<ApplicationDbContext> _unitOfWork;
        private readonly IOpenIddictApplicationManager _applicationManager;

        public AuthentificationController(IUnitOfWork<ApplicationDbContext> uow, IOpenIddictApplicationManager applicationManager)
        {
            _unitOfWork = uow;
            _applicationManager = applicationManager;
        }

        [HttpPost("~/connect/token"), Produces("application/json")]
        public async Task<IActionResult> Exchange()
        {
            var request = HttpContext.GetOpenIddictServerRequest() ??
                          throw new InvalidOperationException("The OpenID Connect request cannot be retrieved.");

            ClaimsPrincipal claimsPrincipal;
            var sha = SHA256.Create();

            if (request.IsPasswordGrantType())
            {
                // Note: the client credentials are automatically validated by OpenIddict:
                var user = await _unitOfWork.GetRepository<User>().GetFirstOrDefault(u => u.Mail == request.Username);
                // if client_id or client_secret are invalid, this action won't be invoked.

                // If player doesn't exist return BadRequest
                if (user == null)
                    return BadRequest("Email and password don't match");

                // Password check
                // If password are not corresponding return BadRequest
                var passwordSaltedH = sha.ComputeHash(Encoding.ASCII.GetBytes(request.Password + Convert.ToBase64String(user.PasswordSalt)));
                if (!user.PasswordHash.SequenceEqual(passwordSaltedH))
                {
                    return BadRequest("Email and password don't match");
                }

                var application = await _applicationManager.FindByClientIdAsync(request.ClientId) ??
                                  throw new InvalidOperationException("The application cannot be found.");

                var identity = new ClaimsIdentity(
                    TokenValidationParameters.DefaultAuthenticationType,
                    OpenIddictConstants.Claims.Name, OpenIddictConstants.Claims.Role);

                // Use the client_id as the subject identifier.
                identity.AddClaim(OpenIddictConstants.Claims.Subject,
                    await _applicationManager.GetClientIdAsync(application),
                    OpenIddictConstants.Destinations.AccessToken);

                identity.AddClaim(OpenIddictConstants.Claims.CodeHash,
                    user.Id.ToString(),
                    OpenIddictConstants.Destinations.AccessToken);

                identity.AddClaim(OpenIddictConstants.Claims.Name,
                    $"{user.FirstName} {user.LastName}",
                    OpenIddictConstants.Destinations.AccessToken);

                identity.AddClaim(OpenIddictConstants.Claims.Email,
                    user.Mail,
                    OpenIddictConstants.Destinations.AccessToken);

                claimsPrincipal = new ClaimsPrincipal(identity);
                claimsPrincipal.SetScopes(request.GetScopes());
            }
            else if (request.IsAuthorizationCodeGrantType())
            {
                // Retrieve the claims principal stored in the authorization code
                claimsPrincipal =
                    (await HttpContext.AuthenticateAsync(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme))
                    .Principal;
            }
            else if (request.IsRefreshTokenGrantType())
            {
                // Retrieve the claims principal stored in the refresh token.
                claimsPrincipal =
                    (await HttpContext.AuthenticateAsync(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme))
                    .Principal;
            }
            else
            {
                throw new InvalidOperationException("The specified grant type is not supported.");
            }

            // Returning a SignInResult will ask OpenIddict to issue the appropriate access/identity tokens.
            return SignIn(claimsPrincipal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }

        [HttpGet("~/connect/authorize")]
        [HttpPost("~/connect/authorize")]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> Authorize()
        {
            var request = HttpContext.GetOpenIddictServerRequest() ??
                          throw new InvalidOperationException("The OpenID Connect request cannot be retrieved.");

            // Retrieve the user principal stored in the authentication cookie.
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // If the user principal can't be extracted, redirect the user to the login page.
            if (!result.Succeeded)
            {
                return Challenge(
                    authenticationSchemes: CookieAuthenticationDefaults.AuthenticationScheme,
                    properties: new AuthenticationProperties
                    {
                        RedirectUri = Request.PathBase + Request.Path + QueryString.Create(
                            Request.HasFormContentType ? Request.Form.ToList() : Request.Query.ToList())
                    });
            }

            // Create a new claims principal
            var claims = new List<Claim>
            {
                // 'subject' claim which is required
                new(OpenIddictConstants.Claims.Subject, result.Principal.Identity.Name),
                new Claim("some claim", "some value").SetDestinations(OpenIddictConstants.Destinations.AccessToken)
            };

            var claimsIdentity = new ClaimsIdentity(claims, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);

            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            // Set requested scopes (this is not done automatically)
            claimsPrincipal.SetScopes(request.GetScopes());

            // Signing in with the OpenIddict authentiction scheme trigger OpenIddict to issue a code (which can be exchanged for an access token)
            return SignIn(claimsPrincipal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }

        [HttpPost("~/connect/register"), Produces("application/json")]
        public async Task<IActionResult> SignUp(AuthentificationVM userVM)
        {
            if (await _unitOfWork.GetRepository<User>().Exists(u => u.Mail == userVM.Mail))
            {
                return Conflict(new { Message = "L'adresse mail existe déja" });
            }
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                return BadRequest(errors);
            }
            var sha = SHA256.Create();
            User user = new User
            {
                FirstName = userVM.FirstName,
                LastName = userVM.LastName,
                BirthDate = userVM.BirthDate,
                Mail = userVM.Mail,
                IsConnected = false
            };
            user.PasswordSalt = new byte[10];
            new RNGCryptoServiceProvider().GetNonZeroBytes(user.PasswordSalt);
            var passwordSaltedH = sha.ComputeHash(Encoding.ASCII.GetBytes(userVM.Password + Convert.ToBase64String(user.PasswordSalt)));
            user.PasswordHash = passwordSaltedH;


            return
            await _unitOfWork.GetRepository<User>().Add(user) ?
            Ok() : BadRequest();
        }

        [HttpGet("~/connect/userinfo")]
        public async Task<IActionResult> UserInfo()
        {
            var claimsPrincipal = (await HttpContext.AuthenticateAsync(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme)).Principal;

            return Ok(new
            {
                Subject = claimsPrincipal.GetClaim(OpenIddictConstants.Claims.Subject),
                Email = claimsPrincipal.GetClaim(OpenIddictConstants.Claims.Email)
            });
        }
    }
}
