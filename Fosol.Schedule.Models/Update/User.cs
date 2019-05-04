using System;
using System.Collections.Generic;

namespace Fosol.Schedule.Models.Update
{
  public class User : BaseModel
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
    /// get/set - The user information.
    /// </summary>
    public UserInfo Info { get; set; }

    /// <summary>
    /// get/set - Foreign key to the default account this user signs into.
    /// </summary>
    public int? DefaultAccountId { get; set; }

    /// <summary>
    /// get - A collection of all the roles this user is part of.
    /// </summary>
    public IList<AccountRole> Roles { get; set; }

    /// <summary>
    /// get - A collection of user contact information.
    /// </summary>
    public IList<ContactInfo> ContactInformation { get; set; }

    /// <summary>
    /// get - A collection of attributes for this user.
    /// </summary>
    public IList<Attribute> Attributes { get; set; }

    /// <summary>
    /// get - A collection of settings for this user.
    /// </summary>
    public IList<UserSetting> Settings { get; set; }
    #endregion
  }
}