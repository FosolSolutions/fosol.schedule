using Fosol.Schedule.DAL.Interfaces;

namespace Fosol.Schedule.DAL.Services
{
  /// <summary>
  /// AccountService sealed class, provides a way to manage accounts in the datasource.
  /// </summary>
  public sealed class AccountService : UpdatableService<Entities.Account, Models.Create.Account, Models.Read.Account, Models.Update.Account, Models.Delete.Account>, IAccountService
  {
    #region Variables
    #endregion

    #region Properties
    #endregion

    #region Constructors
    /// <summary>
    /// Creates a new instance of a AccountService object, and initalizes it with the specified options.
    /// </summary>
    /// <param name="source"></param>
    internal AccountService(IDataSource source) : base(source)
    {
    }
    #endregion

    #region Methods
    /// <summary>
    /// Get the account for the specified 'id'.
    /// Validates whether the current user is authorized to view the account.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Models.Read.Account Get(int id)
    {
      return this.Find(id);
    }
    #endregion
  }
}
