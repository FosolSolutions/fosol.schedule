using System;
using System.Collections.Generic;

namespace Fosol.Schedule.Models.Update
{
  public class Account : BaseModel
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
    /// get/set - The account's business address.
    /// </summary>
    public Address BusinessAddress { get; set; }

    /// <summary>
    /// get/set - The account's business phone.
    /// </summary>
    public string BusinessPhone { get; set; }

    /// <summary>
    /// get/set - The account's toll-free phone.
    /// </summary>
    public string TollFreeNumber { get; set; }

    /// <summary>
    /// get/set - The account's fax number.
    /// </summary>
    public string FaxNumber { get; set; }

    /// <summary>
    /// get/set - The account's email adddress.
    /// </summary>
    public string Email { get; set; }
    #endregion
  }
}