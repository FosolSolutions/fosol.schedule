namespace Fosol.Schedule.DAL.Interfaces
{
  public interface IAccountService : IUpdatableService<Models.Create.Account, Models.Read.Account, Models.Update.Account, Models.Delete.Account>
  {
    /// <summary>
    /// Get the account for the specified 'id'.
    /// Validates whether the current user is authorized to view the account.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Models.Read.Account Get(int id);
  }
}