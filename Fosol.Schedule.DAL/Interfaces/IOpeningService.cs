namespace Fosol.Schedule.DAL.Interfaces
{
	public interface IOpeningService : IUpdatableService<Models.Opening>
	{
		/// <summary>
		/// Get the opening for the specified 'id'.
		/// Validates whether the current user is authorized to view the opening.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		Models.Opening Get(int id);

		/// <summary>
		/// The participant is apply for the opening.
		/// </summary>
		/// <param name="application"></param>
		/// <param name="participant"></param>
		/// <returns></returns>
		Models.Opening Apply(Models.OpeningApplication application, Models.Participant participant = null);

		/// <summary>
		/// The participant is unapply to the opening.
		/// </summary>
		/// <param name="opening"></param>
		/// <param name="participants"></param>
		/// <returns></returns>
		Models.Opening Unapply(Models.Opening opening, params Models.Participant[] participants);
	}
}