using System;
using System.Collections.Generic;

namespace Fosol.Schedule.Models.Update
{
  public class Subscription : BaseModel
  {
    #region Properties
    /// <summary>
    /// get/set - Primary key uses IDENTITY.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// get/set - A unique key to identify this subscription.
    /// </summary>
    public Guid Key { get; set; }

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
    public SubscriptionState State { get; set; } = SubscriptionState.Enabled;
    #endregion
  }
}