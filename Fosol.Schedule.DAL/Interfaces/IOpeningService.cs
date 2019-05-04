using System;
using System.Collections.Generic;

namespace Fosol.Schedule.DAL.Interfaces
{
  public interface IOpeningService : IUpdatableService<Models.Create.Opening, Models.Read.Opening, Models.Update.Opening, Models.Delete.Opening>
  {
    /// <summary>
    /// Get the opening for the specified 'id'.
    /// Validates whether the current user is authorized to view the opening.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Models.Read.Opening Get(int id);

    /// <summary>
    /// Get the openings for the specified 'calendarId' and within the specified timeframe.
    /// Validates whether the current use is authorized to view the calendar.
    /// </summary>
    /// <param name="calendarId"></param>
    /// <param name="startOn"></param>
    /// <param name="endOn"></param>
    /// <returns></returns>
    IEnumerable<Models.Read.Opening> GetForCalendar(int calendarId, DateTime startOn, DateTime endOn);

    /// <summary>
    /// The participant is apply for the opening.
    /// </summary>
    /// <param name="application"></param>
    /// <param name="participant"></param>
    /// <returns></returns>
    Models.Read.Opening Apply(Models.Read.OpeningApplication application, Models.Read.Participant participant = null);

    /// <summary>
    /// The participant is unapply to the opening.
    /// </summary>
    /// <param name="opening"></param>
    /// <param name="participants"></param>
    /// <returns></returns>
    Models.Read.Opening Unapply(Models.Read.Opening opening, params Models.Read.Participant[] participants);
  }
}