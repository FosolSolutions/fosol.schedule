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

namespace Fosol.Schedule.API.Controllers
{
    /// <summary>
    /// AuthController class, provides a way to authenticate a user.
    /// </summary>
    [Produces("application/json")]
    [Route("api/Auth")]
    public class AuthController : Controller
    {
        #region Variables
        private readonly IDataSource _datasource;
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of an AuthController object, and initializes it with the specified arguments.
        /// </summary>
        /// <param name="datasource"></param>
        public AuthController(IDataSource datasource)
        {
            _datasource = datasource;
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
        [HttpGet("validate/participant/{key}")]
        public async Task<IActionResult> ValidateParticipant(Guid key)
        {
            var identity = _datasource.Participants.CreateIdentity(key);
            if (identity == null)
                return Unauthorized();

            var user = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, user);

            return Ok();
        }

        /// <summary>
        /// Signoff the application, clear cookies and session.
        /// </summary>
        /// <returns></returns>
        [HttpGet("signoff"), Authorize]
        public async Task<IActionResult> SignOff()
        {
            await HttpContext.SignOutAsync();

            return Ok();
        }

        /// <summary>
        /// Login with a default participant account.
        /// </summary>
        /// <returns></returns>
        [HttpGet("backdoor")]
        public async Task<IActionResult> Backdoor()
        {
            var user = _datasource.Users.Get(1);
            if (user == null)
                return BadRequest();

            var identity = _datasource.Users.CreateIdentity(user.Key);
            if (identity == null)
                return Unauthorized();

            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            return Ok();
        }

        //[HttpGet("impersonate"), Authorize]
        //public IActionResult Impersonate()
        //{
        //    var participants = _datasource.Participants.All();
        //    return View(participants);
        //}

        /// <summary>
        /// Signs the current user in as the specified participant.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [HttpGet("impersonate/participant/{key}"), Authorize]
        public async Task<IActionResult> ImpersonateParticipant(Guid key)
        {
            var userId = int.Parse(User.GetNameIdentifier().Value);
            var participant = _datasource.Participants.Get(key);
            if (participant == null)
                return BadRequest();

            var identity = _datasource.Participants.CreateIdentity(key);
            if (identity == null)
                return Unauthorized();

            var id = identity.GetNameIdentifier();
            identity.RemoveClaim(id);
            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, $"{userId}"));
            identity.AddClaim(new Claim("Impersonator", $"{participant.Id}", "int", "Fosol.Schedule"));
            var user = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, user);

            return Ok();
        }
        #endregion
    }
}