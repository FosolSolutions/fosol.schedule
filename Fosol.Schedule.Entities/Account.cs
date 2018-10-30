using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fosol.Schedule.Entities
{
    /// <summary>
    /// Account class, provides a way to manage account information in the datasource.
    /// </summary>
    public class Account : BaseEntity
    {
        #region Properties
        /// <summary>
        /// get/set - Primary key uses IDENITTY.
        /// </summary>
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// get/set - A unique key to identify this account.
        /// </summary>
        [Required]
        public Guid Key { get; set; }

        /// <summary>
        /// get/set - Foreign key to the user who owns this account.
        /// </summary>
        public int OwnerId { get; set; }

        /// <summary>
        /// get/set - The owner of this account.
        /// </summary>
        [ForeignKey(nameof(OwnerId))]
        public User Owner { get; set; }

        /// <summary>
        /// get/set - The current state of this account.
        /// </summary>
        public AccountState State { get; set; } = AccountState.Enabled;

        /// <summary>
        /// get/set - The kind of account [Personal, Business]
        /// </summary>
        public AccountKind Kind { get; set; }

        /// <summary>
        /// get/set - Foreign key to the subscription for this account [Free|...].
        /// </summary>
        public int SubscriptionId { get; set; }

        /// <summary>
        /// get/set - The current subscription for this account.
        /// </summary>
        [ForeignKey(nameof(SubscriptionId))]
        public Subscription Subscription { get; set; }

        /// <summary>
        /// get - A collection of users associated with this account.
        /// </summary>
        public ICollection<AccountUser> Users { get; private set; } = new List<AccountUser>();

        /// <summary>
        /// get - A collection of all the roles for this account.
        /// </summary>
        public ICollection<AccountRole> Roles { get; private set; } = new List<AccountRole>();
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of an Account object.
        /// </summary>
        public Account()
        {

        }

        /// <summary>
        /// Creates a new instance of an Account object, and initializes it with the specified properties.
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="kind"></param>
        /// <param name="subscription"></param>
        public Account(User owner, AccountKind kind, Subscription subscription)
        {
            this.OwnerId = owner?.Id ?? throw new ArgumentNullException(nameof(owner));
            this.Owner = owner;
            this.SubscriptionId = subscription?.Id ?? throw new ArgumentNullException(nameof(subscription));
            this.Subscription = subscription;
            this.Kind = kind;
            this.Key = Guid.NewGuid();
        }
        #endregion
    }
}
