using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Fosol.Core.Extensions.ClaimsIdentities;
using Fosol.Core.Extensions.Principals;
using Fosol.Schedule.API.Helpers;
using Fosol.Schedule.DAL.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Fosol.Schedule.API.Controllers
{
    /// <summary>
    /// AuthController class, provides a way to authenticate a user.
    /// </summary>
    [Produces("application/json")]
    [Area("api")]
    [Route("[area]/[controller]")]
    public class AuthController : Controller
    {
        #region Variables
        private readonly ILogger _logger;
        private readonly IDataSource _datasource;
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of an AuthController object, and initializes it with the specified arguments.
        /// </summary>
        /// <param name="datasource"></param>
        /// <param name="logger"></param>
        public AuthController(IDataSource datasource, ILogger<AuthController> logger)
        {
            _datasource = datasource;
            _logger = logger;
        }
        #endregion

        #region Endpoints
        /// <summary>
        /// Returns a view for signin.
        /// </summary>
        /// <returns></returns>
        [HttpGet("signin")]
        public IActionResult Signin()
        {
            return View();
        }

        /// <summary>
        /// Validate the key and sign the participant in.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [HttpGet("signin/participant/{key}")]
        public async Task<IActionResult> SigninParticipant(Guid key)
        {
            var identity = _datasource.Participants.CreateIdentity(key);
            if (identity == null)
                return Unauthorized();

            var id = int.Parse(identity.GetNameIdentifier().Value);
            _logger.LogInformation($"Participant '{id}' signed in.");

            var participant = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, participant);

            return Ok(true);
        }

        /// <summary>
        /// Makes the specified calendar the active calendar for the currently signed in user.
        /// Updates the users claims.
        /// </summary>
        /// <param name="calendarId"></param>
        /// <returns></returns>
        [HttpPut("calendar/{calendarId}")]
        public async Task<IActionResult> SelectCalendar(int calendarId)
        {
            _datasource.Calendars.SelectCalendar(User, calendarId);
            var principal = new ClaimsPrincipal(User);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            return Ok(true);
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
        /// Login with a default user account.
        /// </summary>
        /// <returns></returns>
        [HttpGet("backdoor/user")]
        public async Task<IActionResult> BackdoorUser()
        {
            var user = _datasource.Users.Get(1);
            if (user == null)
                return BadRequest();

            var identity = _datasource.Users.CreateIdentity(user.Key);
            if (identity == null)
                return Unauthorized();

            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            return Ok(true);
        }

        /// <summary>
        /// Login with a default participant account.
        /// </summary>
        /// <returns></returns>
        [HttpGet("backdoor/participant")]
        public async Task<IActionResult> BackdoorParticipant()
        {
            var participant = _datasource.Participants.Get(1);
            if (participant == null)
                return BadRequest();

            var identity = _datasource.Users.CreateIdentity(participant.Key);
            if (identity == null)
                return Unauthorized();

            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            return Ok(true);
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
            var userId = int.Parse(User.GetNameIdentifier().Value);
            var participant = _datasource.Participants.Get(key);
            if (participant == null)
                return BadRequest();

            var identity = _datasource.Participants.CreateIdentity(key);
            if (identity == null)
                return Unauthorized();

            _logger.LogInformation($"Impersonating participant '{participant.Id}' - current user: '{userId}'.");

            var id = identity.GetNameIdentifier();
            identity.RemoveClaim(id);
            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, $"{participant.Id}"));
            identity.AddClaim(new Claim("Impersonator", $"{userId}", "int", "Fosol.Schedule"));

            var impersonate = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, impersonate);

            return Ok(true);
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
            var userId = int.Parse(User.GetNameIdentifier().Value);
            var user = _datasource.Users.Get(key);
            if (user == null)
                return BadRequest();

            var identity = _datasource.Users.CreateIdentity(key);
            if (identity == null)
                return Unauthorized();

            _logger.LogInformation($"Impersonating user '{user.Id}' - current user: '{userId}'.");

            var id = identity.GetNameIdentifier();
            identity.RemoveClaim(id);
            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, $"{user.Id}"));
            identity.AddClaim(new Claim("Impersonator", $"{userId}", "int", "Fosol.Schedule"));

            var impersonate = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, impersonate);

            return Ok(true);
        }
        #endregion
    }
}