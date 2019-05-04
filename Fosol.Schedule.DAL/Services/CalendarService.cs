using Fosol.Core.Exceptions;
using Fosol.Schedule.DAL.Interfaces;
using Fosol.Schedule.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Fosol.Schedule.DAL.Services
{
  /// <summary>
  /// CalendarService sealed class, provides a way to manage calendars in the datasource.
  /// </summary>
  public sealed class CalendarService : UpdatableService<Calendar, Models.Create.Calendar, Models.Read.Calendar, Models.Update.Calendar, Models.Delete.Calendar>, ICalendarService
  {
    #region Variables
    #endregion

    #region Properties
    #endregion

    #region Constructors
    /// <summary>
    /// Creates a new instance of a CalendarService object, and initalizes it with the specified options.
    /// </summary>
    /// <param name="source"></param>
    internal CalendarService(IDataSource source) : base(source)
    {
    }
    #endregion

    #region Methods
    /// <summary>
    /// Get all the calendars owned by the current user.
    /// </summary>
    /// <param name="skip"></param>
    /// <param name="take"></param>
    /// <returns></returns>
    public IEnumerable<Models.Read.Calendar> Get(int skip, int take)
    {
      var userId = this.GetUserId();
      var participantId = this.GetParticipantId();
      return this.Context.Calendars.AsNoTracking().Where(c => c.Account.OwnerId == userId || c.Participants.Any(p => p.Id == participantId)).Skip(skip).Take(take).ToArray().Select(c => this.Source.Map<Models.Read.Calendar>(c)).ToArray();
    }

    /// <summary>
    /// Get the calendar for the specified 'id'.
    /// Validates whether the current user is authorized to view the calendar.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Models.Read.Calendar Get(int id)
    {
      var userId = this.GetUserId();
      var participantId = this.GetParticipantId();
      return this.Source.Map<Models.Read.Calendar>(this.Find((set) => set.Include(c => c.Criteria).Include(c => c.Tags).SingleOrDefault(c => c.Id == id && c.Account.OwnerId == userId || c.Participants.Any(p => p.Id == participantId))));
    }

    /// <summary>
    /// Get the calendar for the specified 'id'.
    /// Validates whether the current user is authorized to view the calendar.
    /// Includes events for the specified timeframe.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="startOn"></param>
    /// <param name="endDate"></param>
    /// <returns></returns>
    public Models.Read.Calendar Get(int id, DateTime startOn, DateTime endOn)
    {
      // Convert datetime to utc.
      var start = startOn.ToUniversalTime();
      var end = endOn.ToUniversalTime();

      var calendar = Get(id);
      var events = this.Context.Events.Where(e => e.CalendarId == id && e.StartOn >= start && e.EndOn <= end).OrderBy(e => e.StartOn).ThenBy(e => e.Name);

      calendar.Events = events.Select(e => this.Source.Map<Models.Read.Event>(e, null));

      return calendar;
    }

    /// <summary>
    /// Add the specified calendar to the datasource.
    /// Validates whether the current user is authorized to do this.
    /// </summary>
    /// <param name="model"></param>
    public override Models.Update.Calendar Add(Models.Create.Calendar model)
    {
      if (model == null) throw new ArgumentNullException(nameof(model));
      var entity = this.Source.Map<Entities.Calendar>(model);
      entity.AccountId = this.GetAccountId();
      return base.Add(entity);
    }

    /// <summary>
    /// Update the specified calendar in the datasource.
    /// Validates whether the current user is authorized to do this.
    /// </summary>
    /// <param name="model"></param>
    public override Models.Update.Calendar Update(Models.Update.Calendar model)
    {
      if (model == null) throw new ArgumentNullException(nameof(model));

      // Must own the account.
      // TODO: Permission based action.
      var userId = this.GetUserId();
      var calendar = this.Find(model);
      var accountId = this.GetAccountId();
      var ownsAccount = this.Context.Accounts.Any(a => a.Id == calendar.AccountId && a.OwnerId == userId);
      if (!ownsAccount) throw new NotAuthorizedException();

      var entity = this.Source.Map<Entities.Calendar>(model);
      entity.AccountId = accountId;

      return base.Update(entity);
    }

    /// <summary>
    /// Remove the specified calendar from the datasource.
    /// Validates whether the current user is authorized to do this.
    /// </summary>
    /// <param name="model"></param>
    public override void Remove(Models.Delete.Calendar model)
    {
      if (model == null) throw new ArgumentNullException(nameof(model));

      // Must own the account.
      // TODO: Permission based action.
      var userId = this.GetUserId();
      var calendar = this.Find(model);
      var ownsAccount = this.Context.Accounts.Any(a => a.Id == calendar.AccountId && a.OwnerId == userId);
      if (!ownsAccount) throw new NotAuthorizedException();

      base.Remove(model);
    }

    /// <summary>
    /// Get the claims associated with this calendar.
    /// Validates whether the current user  is authorized to view the calendar.
    /// </summary>
    /// <param name="calendarId"></param>
    /// <returns></returns>
    public IEnumerable<Claim> GetClaims(int calendarId)
    {
      var id = this.GetParticipantId() ?? this.GetUserId();
      var isParticipant = this.IsPrincipalAParticipant;
      var calendar = this.Context.Calendars.SingleOrDefault(c => c.Id == calendarId && (isParticipant && c.Participants.Any(p => p.Id == id) || c.Account.Users.Any(u => u.UserId == id))) ?? throw new NotAuthorizedException();
      var participant = this.Context.Participants.SingleOrDefault(p => isParticipant && p.Id == id || (p.CalendarId == calendarId && p.UserId == id)) ?? throw new InvalidOperationException($"User must first become a participant in this calendar."); // TODO: New workflow, if a user isn't a participant it should redirect them to a page to become one.
      var claims = new List<Claim>(new[]
      {
        new Claim("Calendar", $"{calendarId}", typeof(int).FullName, "CoEvent"),
        new Claim("Account", $"{calendar.AccountId}", typeof(int).FullName, "CoEvent"),
        new Claim("Participant", $"{participant.Id}", typeof(int).FullName, "CoEvent")
      });

      return claims;
    }
    #endregion
  }
}
