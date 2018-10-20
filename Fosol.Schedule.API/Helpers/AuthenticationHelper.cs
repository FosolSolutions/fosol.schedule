using Fosol.Core.Extensions.Enumerable;
using Fosol.Schedule.DAL.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using System;
using System.Linq;
using System.Security.Claims;

namespace Fosol.Schedule.API.Helpers
{
    /// <summary>
    /// AuthenticationHelper static class, provides helper methods for authentication.
    /// </summary>
    public static class AuthenticationHelper
    {
        #region Methods
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
        #endregion
    }
}
