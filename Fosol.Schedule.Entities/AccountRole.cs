using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// get/set - A unique name to identify this account role.
        /// </summary>
        [Required, MaxLength(100)]
        public string Name { get; set; }

        /// <summary>
        /// get/set - A description of this account role.
        /// </summary>
        [MaxLength(2000)]
        public string Description { get; set; }

        /// <summary>
        /// get/set - The privileges this account role grants to a user.
        /// </summary>
        public AccountPrivilege Privileges { get; set; }

        /// <summary>
        /// get/set - Foreign key to the account this role belongs to.
        /// </summary>
        public int AccountId { get; set; }

        /// <summary>
        /// get/set - The account this role belongs to.
        /// </summary>
        [ForeignKey(nameof(AccountId))]
        public Account Account { get; set; }

        /// <summary>
        /// get - A collection of all the users in this account that have this role.
        /// </summary>
        public ICollection<UserAccountRole> UserAccountRoles { get; set; } = new List<UserAccountRole>();
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of an AccountRole object.
        /// </summary>
        public AccountRole()
        { }

        /// <summary>
        /// Creates a new instance of an AccountRole object, and initializes with the specified arguments.
        /// </summary>
        /// <param name="account"></param>
        /// <param name="name"></param>
        /// <param name="privileges"></param>
        public AccountRole(Account account, string name, AccountPrivilege privileges)
        {
            if (String.IsNullOrWhiteSpace(name))
                throw new ArgumentException($"Argument '{nameof(name)}' is required and cannot be nullable or empty.");

            this.AccountId = account?.Id ?? throw new ArgumentNullException(nameof(account));
            this.Account = account;
            this.Name = name;
            this.Privileges = privileges;
        }
        #endregion
    }
}
