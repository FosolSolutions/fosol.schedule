using Fosol.Core.Exceptions;
using Fosol.Core.Extensions.Enumerable;
using Fosol.Core.Extensions.Principals;
using Fosol.Core.Mvc;
using Fosol.Schedule.DAL.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Fosol.Schedule.API.Helpers
{
    /// <summary>
    /// AuthenticationHelper static class, provides helper methods for authentication.
    /// </summary>
    public static class AuthenticationHelper
    {
        #region Methods
        // TODO: Replace with better implementation, use built-in error handling.
        /// <summary>
        /// When an oath authorization or token request fails.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static async Task HandleOnRemoteFailure(RemoteFailureContext context)
        {
            var handler = context.HttpContext.RequestServices.GetRequiredService<JsonErrorHandler>();

            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(handler.Serialize(new OauthException(context.Failure)));
            context.HandleResponse();
        }

        /// <summary>
        /// When an oauth token has been successfully received.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static async Task HandleOnTicketReceived(TicketReceivedContext context)
        {
            await Task.Run(() =>
            {
                var datasource = context.HttpContext.RequestServices.GetRequiredService<IDataSource>();
                AuthenticationHelper.AuthorizeOauthUser(datasource, context.Principal);
            });
            context.Success();
        }

        /// <summary>
        /// Creates an identity object for a participant that matches the specified 'key'.
        /// </summary>
        /// <param name="service"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static ClaimsIdentity CreateIdentity(this IParticipantService service, Guid key)
        {
            if (key == Guid.Empty)
                return null;

            var participant = service.Get(key);
            var claims = service.GetClaims(participant.Id);
            return new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        }

        /// <summary>
        /// Creates an identity object for a participant that matches the specified 'key'.
        /// </summary>
        /// <param name="service"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static ClaimsIdentity CreateIdentity(this IUserService service, Guid key)
        {
            if (key == Guid.Empty)
                return null;

            var user = service.Get(key);
            var claims = service.GetClaims(user.Id);
            return new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        }

        /// <summary>
        /// When a user selects a calendar it must update the principal claims.
        /// </summary>
        /// <param name="service"></param>
        /// <param name="principal"></param>
        /// <param name="calendarId"></param>
        public static void SelectCalendar(this ICalendarService service, ClaimsPrincipal principal, int calendarId)
        {
            var claims = service.GetClaims(calendarId);
            var identity = principal.Identity as ClaimsIdentity;
            claims.ForEach(c =>
            {
                var claim = identity.Claims.FirstOrDefault(cl => cl.Type == c.Type);
                if (claim != null) identity?.TryRemoveClaim(claim);
            });
            identity?.AddClaims(claims);
        }

        /// <summary>
        /// Authorize the user which was authenticated with Oauth.
        /// </summary>
        /// <param name="datasource"></param>
        /// <param name="principal"></param>
        public static void AuthorizeOauthUser(this IDataSource datasource, ClaimsPrincipal principal)
        {
            // TODO: Allow a user to have multiple Oauth accounts from different providers that link to the internal user by it's email account(s).
            // TODO: Redirect user to create a User Profile page.
            var email = principal.GetEmail()?.Value;
            var userId = datasource.Users.Verify(email);
            Schedule.Models.User user;
            if (userId == 0)
            {
                user = new Schedule.Models.User()
                {
                    Key = Guid.NewGuid(),
                    LastName = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Surname)?.Value,
                    FirstName = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.GivenName)?.Value,
                    Email = email,
                    OauthAccounts = new List<Schedule.Models.OauthAccount>(new[] 
                    {
                        new Schedule.Models.OauthAccount()
                        {
                            Key = principal.GetNameIdentifier()?.Value,
                            Email = email,
                            Issuer = principal.GetNameIdentifier().Issuer
                        }
                    })
                };
                datasource.Users.Add(user);
                datasource.CommitTransaction();
            }
            else
            {
                user = datasource.Users.Get(userId);
            }

            var identity = principal.Identity as ClaimsIdentity;
            var claims = new List<Claim>(new[]
            {
                new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}", typeof(string).FullName, "CoEvent"), // TODO: Issuer namespace.
                new Claim("User", $"{user.Id}", typeof(int).FullName, "CoEvent"),
                new Claim("Account", $"{user.DefaultAccountId ?? user.OwnedAccounts.FirstOrDefault()?.Id ?? 0}", typeof(int).FullName, "CoEvent")
            });
            claims.ForEach(c =>
            {
                var claim = identity.Claims.FirstOrDefault(cl => cl.Type == c.Type);
                if (claim != null) identity?.TryRemoveClaim(claim);
            });
            identity?.AddClaims(claims);
        }
        #endregion
    }
}
