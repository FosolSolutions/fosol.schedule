namespace Fosol.Schedule.DAL.Interfaces
{
  public interface ISubscriptionService : IUpdatableService<Models.Create.Subscription, Models.Read.Subscription, Models.Update.Subscription, Models.Delete.Subscription>
  {
    /// <summary>
    /// Get the subscription for the specified 'id'.
    /// Validates whether the current user is authorized to view the subscription.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Models.Read.Subscription Get(int id);
  }
}