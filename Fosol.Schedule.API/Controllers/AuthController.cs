using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Fosol.Core.Extensions.ClaimsIdentities;
using Fosol.Core.Extensions.Principals;
using Fosol.Core.Extensions.Strings;
using Fosol.Schedule.API.Helpers;
using Fosol.Schedule.DAL.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Fosol.Schedule.API.Controllers
{
    /// <summary>
    /// AuthController class, provides a way to authenticate a user.
    /// </summary>
    [Produces("application/json")]
    [Route("[controller]")]
    public class AuthController : Controller
    {
        #region Variables
        private readonly ILogger _logger;
        private readonly IDataSource _dataSource;
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of an AuthController object, and initializes it with the specified arguments.
        /// </summary>
        /// <param name="dataSource"></param>
        /// <param name="logger"></param>
        public AuthController(IDataSource dataSource, ILogger<AuthController> logger)
        {
            _dataSource = dataSource;
            _logger = logger;
        }
        #endregion

        #region Endpoints
        /// <summary>
        /// If the query does not contain an 'authSchema' it will return a view with a signin form.
        /// If the query contains an 'authSchema' it will redirect to the 3rd party oauth provider authentication form.
        /// </summary>
        /// <param name="authScheme">The 3rd party oauth provider name [Microsoft|Google|Facebook|...].</param>
        /// <param name="redirectUrl">The URL to redirec to after successfully signing in.</param>
        /// <returns></returns>
        [HttpGet("signin")]
        public async Task<IActionResult> Signin([FromQuery] string authScheme, [FromQuery] string redirectUrl = "/api/endpoints")
        {
            if (!String.IsNullOrWhiteSpace(authScheme))
            {
                var authProperties = new AuthenticationProperties() { RedirectUri = redirectUrl };
                return new ChallengeResult(authScheme, authProperties);
            }

            var providers = new List<string>();
            var schemeProvider = this.HttpContext.RequestServices.GetRequiredService<IAuthenticationSchemeProvider>();
            foreach (var provider in await schemeProvider.GetAllSchemesAsync())
            {
                if (!String.IsNullOrWhiteSpace(provider.DisplayName))
                {
                    providers.Add("<a href=\"?authscheme=" + provider.Name + "\">" + (provider.DisplayName ?? "(suppressed)") + "</a>");
                }
            }
            return View(providers);
        }

        /// <summary>
        /// Validate the key and sign the participant in.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [HttpGet("signin/participant/{key}")]
        public async Task<IActionResult> SigninParticipant(Guid key)
        {
            var participant = _dataSource.Participants.Get(key);
            var identity = _dataSource.Participants.CreateIdentity(key);
            if (identity == null)
                return Unauthorized();

            var id = identity.GetParticipant().Value.ConvertTo<int>();
            _logger.LogInformation($"Participant '{id}' signed in.");

            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            return Ok(participant);
        }

        /// <summary>
        /// Signoff the application, clear cookies and session.
        /// </summary>
        /// <returns></returns>
        [HttpGet("signoff"), Authorize]
        public async Task<IActionResult> SignOff()
        {
            await HttpContext.SignOutAsync();

            return Ok(true);
        }

        /// <summary>
        /// Occurs when the oauth signin fails because access was denied.
        /// </summary>
        /// <returns></returns>
        [HttpGet("access/denied")]
        public IActionResult AccessDenied()
        {
            return View();
        }

        /// <summary>
        /// Get the current identity of the logged in user.  This may be a particpant or a user depending on how they signed in.
        /// </summary>
        /// <returns></returns>
        [HttpGet("current/identity"), Authorize]
        public IActionResult CurrentPrincipal()
        {
            var participantId = User.GetParticipant()?.Value.ConvertTo<int>();
            var userId = User.GetUser()?.Value.ConvertTo<int>();

            if (userId.HasValue)
            {
                var user = _dataSource.Users.Get(userId.Value);
                return Ok(user);
            }
            else
            {
                var participant = _dataSource.Participants.Get(participantId.Value);
                return Ok(participant);
            }
        }

        /// <summary>
        /// Login with a default user account.
        /// </summary>
        /// <returns></returns>
        [HttpGet("backdoor/user")]
        public async Task<IActionResult> BackdoorUser()
        {
            var user = _dataSource.Users.Get(1);
            if (user == null)
                return BadRequest();

            var identity = _dataSource.Users.CreateIdentity(user.Key.Value);
            if (identity == null)
                return Unauthorized();

            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            return Ok(user);
        }

        /// <summary>
        /// Login with a default participant account.
        /// </summary>
        /// <returns></returns>
        [HttpGet("backdoor/participant")]
        public async Task<IActionResult> BackdoorParticipant()
        {
            var participant = _dataSource.Participants.Get(1);
            if (participant == null)
                return BadRequest();

            var identity = _dataSource.Participants.CreateIdentity(participant.Key.Value);
            if (identity == null)
                return Unauthorized();

            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            return Ok(participant);
        }

        /// <summary>
        /// Signs the current user in as the specified participant.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [HttpGet("impersonate/participant/{key}"), Authorize]
        public async Task<IActionResult> ImpersonateParticipant(Guid key)
        {
            // Participants are not allowed to impersonate.
            if (User.GetParticipant() != null) return Unauthorized();

            // TODO: Verify that the user is allowed to perform this action.
            var userId = User.GetUser().Value.ConvertTo<int>();
            var participant = _dataSource.Participants.Get(key);
            if (participant == null)
                return BadRequest();

            var identity = _dataSource.Participants.CreateIdentity(key);
            if (identity == null)
                return Unauthorized();

            _logger.LogInformation($"Impersonating participant '{participant.Id}' - current user: '{userId}'.");

            var participantClaim = identity.GetParticipant();
            if (participantClaim != null)
            {
                identity.RemoveClaim(participantClaim);
            }

            var impersonateClaim = identity.GetImpersonator();
            if (impersonateClaim != null)
            {
                identity.RemoveClaim(impersonateClaim);
            }
            
            identity.AddClaim(new Claim("Participant", $"{participant.Id}", typeof(int).Name, "CoEvent"));
            identity.AddClaim(new Claim("Impersonator", $"{userId}", typeof(int).Name, "CoEvent")); // TODO: Issuer

            var impersonate = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, impersonate);

            return Ok(participant);
        }

        /// <summary>
        /// Signs the current user in as the specified user.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [HttpGet("impersonate/participant/{key}"), Authorize]
        public async Task<IActionResult> ImpersonateUser(Guid key)
        {
            // Participants are not allowed to impersonate.
            if (User.GetParticipant() != null) return Unauthorized();

            // TODO: Verify that the user is allowed to perform this action.
            var userId = User.GetUser().Value.ConvertTo<int>();
            var user = _dataSource.Users.Get(key);
            if (user == null)
                return BadRequest();

            var identity = _dataSource.Users.CreateIdentity(key);
            if (identity == null)
                return Unauthorized();

            _logger.LogInformation($"Impersonating user '{user.Id}' - current user: '{userId}'.");

            var identifierClaim = identity.GetNameIdentifier();
            if (identifierClaim != null)
            {
                identity.RemoveClaim(identifierClaim);
            }

            var userClaim = identity.GetUser();
            if (userClaim != null)
            {
                identity.RemoveClaim(userClaim);
            }

            var impersonateClaim = identity.GetImpersonator();
            if (impersonateClaim != null)
            {
                identity.RemoveClaim(impersonateClaim);
            }

            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, $"{user.Key}", typeof(Guid).Name, "CoEvent"));
            identity.AddClaim(new Claim("User", $"{user.Id}", typeof(int).Name, "CoEvent"));
            identity.AddClaim(new Claim("Impersonator", $"{userId}", typeof(int).Name, "CoEvent")); // TODO: Issuer

            var impersonate = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, impersonate);

            return Ok(user);
        }
        #endregion
    }
}