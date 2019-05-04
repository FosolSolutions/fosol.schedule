using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace Fosol.Schedule.DAL.Interfaces
{
  public interface IParticipantService : IUpdatableService<Models.Create.Participant, Models.Read.Participant, Models.Update.Participant, Models.Delete.Participant>
  {
    /// <summary>
    /// Get the calendar for the specified 'id'.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Models.Read.Participant Get(int id);

    /// <summary>
    /// Get the calendar for the specified 'key'.
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    Models.Read.Participant Get(Guid key);

    /// <summary>
    /// Get the claimed identity of the participant for the specified 'participantId'.
    /// </summary>
    /// <param name="participantId"></param>
    /// <returns></returns>
    IEnumerable<Claim> GetClaims(int participantId);

    /// <summary>
    /// Get all the participants in the specified calendar.
    /// </summary>
    /// <param name="calendarId"></param>
    /// <param name="skip"></param>
    /// <param name="take"></param>
    /// <returns></returns>
    IEnumerable<Models.Read.Participant> GetForCalendar(int calendarId, int skip = 0, int take = 20);
  }
}