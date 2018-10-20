using Fosol.Core.Exceptions;
using Fosol.Schedule.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Fosol.Schedule.DAL.Services
{
    /// <summary>
    /// UserService sealed class, provides a way to manage users in the datasource.
    /// </summary>
    public sealed class UserService : UpdatableService<Entities.User, Models.User>, IUserService
    {
        #region Variables
        #endregion

        #region Properties
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a UserService object, and initalizes it with the specified options.
        /// </summary>
        /// <param name="source"></param>
        internal UserService(IDataSource source) : base(source)
        {
            //Authenticated();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Get the user for the specified 'key'.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Models.User Get(Guid key)
        {
            return this.Map(this.Find((set) => set.Include(u => u.Info).SingleOrDefault(u => u.Key == key)));
        }

        /// <summary>
        /// Get the user for the specified 'id'.
        /// Validates whether the current user is authorized to view the user.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Models.User Get(int id)
        {
            return this.Map(this.Find((set) => set.Include(u => u.Info).SingleOrDefault(u => u.Id == id)));
        }

        /// <summary>
        /// Get the claimed identity of the user for the specified 'userId'.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public IEnumerable<Claim> GetClaims(int userId)
        {
            var user = this.Find((set) => set.Include(u => u.Attributes).ThenInclude(a => a.Attribute).Include(u => u.DefaultAccount).Include(u => u.Info).SingleOrDefault(u => u.Id == userId));

            var claims = new List<Claim>(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, $"{user.Id}"),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, $"{user.Info.FirstName} {user.Info.LastName}"),
                new Claim(ClaimTypes.Surname, user.Info.LastName ?? ""),
                new Claim(ClaimTypes.Gender, $"{user.Info.Gender}"),
                new Claim("Key", $"{user.Key}", typeof(Guid).FullName, "Fosol.Schedule"),
                new Claim("Account", $"{user.DefaultAccountId}", typeof(int).FullName, "Fosol.Schedule")
            });

            foreach (var attr in user.Attributes)
            {
                claims.Add(new Claim(attr.Attribute.Key, attr.Attribute.Value, attr.Attribute.ValueType, "Fosol.Schedule"));
            }

            return claims;
        }
        #endregion
    }
}
