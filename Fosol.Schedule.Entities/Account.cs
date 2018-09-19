using System;
using System.Collections.Generic;

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
        public int Id { get; set; }

        /// <summary>
        /// get/set - A unique key to identify this account.
        /// </summary>
        public Guid Key { get; set; }

        /// <summary>
        /// get/set - Foreign key to the user who owns this account.
        /// </summary>
        public int OwnerId { get; set; }

        /// <summary>
        /// get/set - The owner of this account.
        /// </summary>
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
        public Subscription Subscription { get; set; }

        /// <summary>
        /// get - A collection of users associated with this account.
        /// </summary>
        public ICollection<User> Users { get; set; } = new List<User>();

        /// <summary>
        /// get - A collection of all the roles for this account.
        /// </summary>
        public ICollection<AccountRole> Roles { get; set; } = new List<AccountRole>();
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
        /// <param name="ownerId"></param>
        /// <param name="kind"></param>
        /// <param name="subscriptionId"></param>
        public Account(int ownerId, AccountKind kind, int subscriptionId)
        {
            this.Key = Guid.NewGuid();
            this.OwnerId = ownerId;
            this.Kind = kind;
            this.SubscriptionId = subscriptionId;
        }
        #endregion
    }
}
