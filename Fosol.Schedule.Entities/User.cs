using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// get/set - A unique key to identify this user.
        /// </summary>
        [Required]
        public Guid Key { get; set; }

        /// <summary>
        /// get/set - A unique email address that identifies this user.
        /// </summary>
        [Required, MaxLength(250)]
        public string Email { get; set; }

        /// <summary>
        /// get/set - The current state of this user.
        /// </summary>
        public UserState State { get; set; } = UserState.Enabled;

        /// <summary>
        /// get/set - The user information.
        /// </summary>
        [ForeignKey(nameof(Id))]
        public UserInfo Info { get; set; }

        /// <summary>
        /// get - A collection of all the acounts owned by this user.
        /// </summary>
        public ICollection<Account> OwnedAccounts { get; set; } = new List<Account>();

        /// <summary>
        /// get - A collection of all the accounts this user is associated with.
        /// </summary>
        public ICollection<AccountUser> AccountUsers { get; set; } = new List<AccountUser>();

        /// <summary>
        /// get - A collection of all the roles this user is part of.
        /// </summary>
        public ICollection<UserAccountRole> UserAccountRoles { get; set; } = new List<UserAccountRole>();

        /// <summary>
        /// get - A collection of all the participants associated with this user.
        /// </summary>
        public ICollection<Participant> Participants { get; set; } = new List<Participant>();

        /// <summary>
        /// get - A collection of user contact information.
        /// </summary>
        public ICollection<ContactInfo> ContactInformation { get; set; } = new List<ContactInfo>();

        /// <summary>
        /// get - A collection of addresses for this user.
        /// </summary>
        public ICollection<UserAddress> UserAddresses { get; set; } = new List<UserAddress>();

        /// <summary>
        /// get - A collection of attributes for this user.
        /// </summary>
        public ICollection<Attribute> Attributes { get; set; } = new List<Attribute>();
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
