using System;
using System.Collections.Generic;
using System.Text;

namespace Fosol.Schedule.Entities
{
    /// <summary>
    /// User class, provides a way to manage user information in the datasource.
    /// </summary>
    public class User : BaseEntity
    {
        #region Properties
        /// <summary>
        /// get/set - Primary key uses IDENTITY.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// get/set - A unique key to identify this user.
        /// </summary>
        public Guid Key { get; set; }

        /// <summary>
        /// get/set - A unique email address that identifies this user.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// get/set - The current state of this user.
        /// </summary>
        public UserState State { get; set; } = UserState.Enabled;

        /// <summary>
        /// get - A collection of all the accounts this user owns.
        /// </summary>
        public ICollection<Account> Accounts { get; set; } = new List<Account>();

        /// <summary>
        /// get - A collection of all the account roles this user is part of.
        /// </summary>
        public ICollection<AccountRole> AccountRoles { get; set; } = new List<AccountRole>();

        /// <summary>
        /// get/set - A collection of all the participants associated with this user.
        /// </summary>
        public ICollection<Participant> Participants { get; set; } = new List<Participant>();
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a User object.
        /// </summary>
        public User()
        { }

        /// <summary>
        /// Creates a new instance of a User object, and initializes it with the specified properties.
        /// </summary>
        /// <param name="email"></param>
        public User(string email)
        {
            if (String.IsNullOrWhiteSpace(email))
                throw new ArgumentNullException(nameof(email));

            this.Key = Guid.NewGuid();
            this.Email = email;
        }
        #endregion
    }
}
