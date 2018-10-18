using Fosol.Schedule.DAL.Interfaces;
using Fosol.Schedule.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using System;
using System.Collections.Generic;
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
        /// Creates an identity for the specified participant.
        /// </summary>
        /// <param name="participant"></param>
        /// <returns></returns>
        private static ClaimsIdentity CreateIdentity(Participant participant)
        {
            if (participant == null)
                return null;

            var claims = new List<Claim>(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, $"{participant.Id}"),
                //new Claim(ClaimTypes.Email, participant.Email),
                new Claim(ClaimTypes.Name, $"{participant.FirstName} {participant.LastName}"),
                new Claim(ClaimTypes.Surname, participant.LastName),
                new Claim(ClaimTypes.Gender, $"{participant.Gender}"),
                new Claim("Key", $"{participant.Key}", "string", "Fosol.Schedule"),
                new Claim("Participant", "true", "boolean", "Fosol.Schedule"),
                new Claim("Calendar", $"{participant.CalendarId}", "Int32", "Fosol.Schedule")
            });

            foreach (var attr in participant.Attributes)
            {
                claims.Add(new Claim(attr.Key, attr.Value, "string", "Fosol.Schedule"));
            }

            return new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
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
            return CreateIdentity(participant);
        }

        /// <summary>
        /// Creates an identity for the specified user.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private static ClaimsIdentity CreateIdentity(User user)
        {
            if (user == null)
                return null;

            var claims = new List<Claim>(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, $"{user.Id}"),
                //new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
                new Claim(ClaimTypes.Surname, user.LastName ?? ""),
                new Claim(ClaimTypes.Gender, $"{user.Gender}"),
                new Claim("Key", $"{user.Key}", "string", "Fosol.Schedule")
            });

            foreach (var attr in user.Attributes)
            {
                claims.Add(new Claim(attr.Key, attr.Value, "string", "Fosol.Schedule"));
            }

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
            return CreateIdentity(user);
        }
        #endregion
    }
}
