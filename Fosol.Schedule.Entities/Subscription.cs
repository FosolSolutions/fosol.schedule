﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Fosol.Schedule.Entities
{
    /// <summary>
    /// Subscription class, provides a way to manage subscription information in the datasource.
    /// </summary>
    public class Subscription : BaseEntity
    {
        #region Properties
        /// <summary>
        /// get/set - Primary key uses IDENTITY.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// get/set - A unique name to identify the subscription.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// get/set - A description of this subscription.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// get/set - The current state of this subscription.
        /// </summary>
        public SubscriptionState State { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a Subscription object.
        /// </summary>
        public Subscription()
        { }

        /// <summary>
        /// Creates a new instance of a Subscription object, and initializes it with the specified properties.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        public Subscription(string name, string description)
        {
            if (String.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name));

            this.Name = name;
            this.Description = description;
        }
        #endregion
    }
}
