using System.Linq;
using System.Security.Claims;
using System.Security.Principal;

namespace Fosol.Core.Extensions.Principals
{
    /// <summary>
    /// PrincipalExtensions static class, provides extension methods for IPrincipal.
    /// </summary>
    public static class PrincipalExtensions
    {
        #region Methods
        /// <summary>
        /// Get the current principal name identifier.
        /// </summary>
        /// <param name="principal"></param>
        /// <returns></returns>
        public static Claim GetNameIdentifier(this IPrincipal principal)
        {
            return (principal as ClaimsPrincipal)?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
        }

        public static Claim GetKey(this IPrincipal principal)
        {
            return (principal as ClaimsPrincipal)?.Claims.FirstOrDefault(c => c.Type == "Key");
        }

        public static Claim GetImpersonator(this IPrincipal principal)
        {
            return (principal as ClaimsPrincipal)?.Claims.FirstOrDefault(c => c.Type == "Impersonator");
        }

        public static Claim GetEmail(this IPrincipal principal)
        {
            return (principal as ClaimsPrincipal)?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);
        }

        public static Claim GetName(this IPrincipal principal)
        {
            return (principal as ClaimsPrincipal)?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);
        }

        public static Claim GetSurname(this IPrincipal principal)
        {
            return (principal as ClaimsPrincipal)?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Surname);
        }

        public static Claim GetGender(this IPrincipal principal)
        {
            return (principal as ClaimsPrincipal)?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Gender);
        }

        public static Claim GetParticipant(this IPrincipal principal)
        {
            return (principal as ClaimsPrincipal)?.Claims.FirstOrDefault(c => c.Type == "Participant");
        }
        #endregion
    }
}
