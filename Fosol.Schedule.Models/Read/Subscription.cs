using System;

namespace Fosol.Schedule.Models.Read
{
  public class Subscription : BaseModel
  {
    #region Properties
    public int? Id { get; set; }

    /// <summary>
    /// get/set - A unique key to identify this subscription.
    /// </summary>
    public Guid? Key { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public SubscriptionState State { get; set; }
    #endregion
  }
}
