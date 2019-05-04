namespace Fosol.Schedule.Models.Update
{
  public class AccountRole : BaseModel
  {
    #region Properties
    /// <summary>
    /// get/set - Primary key uses IDENTITY.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// get/set - A unique name to identify this account role.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// get/set - A description of this account role.
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// get/set - The privileges this account role grants to a user.
    /// </summary>
    public AccountPrivilege Privileges { get; set; }

    /// <summary>
    /// get/set - The state of this role.
    /// </summary>
    public AccountRoleState State { get; set; } = AccountRoleState.Enabled;

    /// <summary>
    /// get/set - Foreign key to the account this role belongs to.
    /// </summary>
    public int AccountId { get; set; }
    #endregion
  }
}