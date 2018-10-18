using Fosol.Core.Exceptions;
using Fosol.Schedule.DAL.Interfaces;
using Fosol.Schedule.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Fosol.Schedule.DAL.Services
{
    /// <summary>
    /// CalendarService sealed class, provides a way to manage calendars in the datasource.
    /// </summary>
    public sealed class CalendarService : UpdatableService<Calendar, Models.Calendar>, ICalendarService
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
            //Authenticated();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Get all the calendars owned by the current user.
        /// </summary>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <returns></returns>
        public IEnumerable<Models.Calendar> Get(int skip, int take)
        {
            var id = this.GetPrincipalId();
            return this.Context.Calendars.Where(c => c.Account.OwnerId == id).Skip(skip).Take(take).ToArray().Select(c => this.Map(c)).ToArray();
        }

        /// <summary>
        /// Get the calendar for the specified 'id'.
        /// Validates whether the current user is authorized to view the calendar.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Models.Calendar Get(int id)
        {
            var calendar = this.Find(id);
            return this.Map(calendar);
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
        public Models.Calendar Get(int id, DateTime startOn, DateTime endOn)
        {
            // Convert datetime to utc.
            var start = startOn.ToUniversalTime();
            var end = endOn.ToUniversalTime();

            var calendar = Get(id);
            var events = this.Context.Events.Where(e => e.CalendarId == id && e.StartOn >= start && e.EndOn <= end);

            calendar.Events = events.Select(e => this.Source.Mapper.Map<Models.Event>(e));

            return calendar;
        }

        /// <summary>
        /// Add the specified calendar to the datasource.
        /// Validates whether the current user is authorized to do this.
        /// </summary>
        /// <param name="model"></param>
        public override void Add(Models.Calendar model)
        {
            this.VerifyPrincipal(true);

            // Must own the account.
            // TODO: Permission based action.
            var userId = this.GetUserId();
            var ownsAccount = this.Context.Accounts.Any(a => a.OwnerId == userId);
            if (!ownsAccount) throw new NotAuthorizedException();

            if (model.Key == Guid.Empty) model.Key = Guid.NewGuid();
            base.Add(model);
        }
        #endregion
    }
}
