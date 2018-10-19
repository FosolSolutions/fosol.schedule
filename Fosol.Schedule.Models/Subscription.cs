using Fosol.Schedule.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fosol.Schedule.Models
{
    public class Subscription : BaseModel
    {
        #region Properties
        public int Id { get; set; }

        /// <summary>
        /// get/set - A unique key to identify this subscription.
        /// </summary>
        public Guid Key { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        /// <summary>
        /// get/set - The current state of this subscription.
        /// </summary>
        public SubscriptionState State { get; set; } = SubscriptionState.Enabled;
        #endregion
    }
}
