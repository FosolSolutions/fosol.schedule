using System;
using System.Collections.Generic;
using System.Text;

namespace Fosol.Schedule.Entities
{
    /// <summary>
    /// AccountRole class, provides a way to manage account roles within the datasource.
    /// </summary>
    public class AccountRole : BaseEntity
    {
        #region Properties
        /// <summary>
        /// get/set - Primary key uses IDENTITY.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// get/set - A unique name to identify this account role.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// get/set - A description of this account role.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// get/set - The privileges this account role grants to a user.
        /// </summary>
        public AccountPrivilege Privilege { get; set; }

        /// <summary>
        /// get/set - Foreign key to the account this role belongs to.
        /// </summary>
        public int AccountId { get; set; }

        /// <summary>
        /// get/set - The account this role belongs to.
        /// </summary>
        public Account Account { get; set; }

        /// <summary>
        /// get - A collection of all the users in this account that have this role.
        /// </summary>
        public ICollection<User> Users { get; set; } = new List<User>();
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instances of an AccountRole object.
        /// </summary>
        public AccountRole()
        { }
        #endregion
    }
}
