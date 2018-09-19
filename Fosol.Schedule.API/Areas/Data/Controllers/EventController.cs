using Fosol.Schedule.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fosol.Schedule.API.Areas.Data.Controllers
{
    /// <summary>
    /// EventController sealed class, provides API endpoints for calendar events.
    /// </summary>
    [Produces("application/json")]
    [Area("data")]
    [Route("[area]/calendar/[controller]")]
    public sealed class EventController : Controller
    {
        #region Variables
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a EventController object.
        /// </summary>
        public EventController()
        {
        }
        #endregion

        #region Methods

        /// <summary>
        /// Returns a calendar event for the specified 'id'.
        /// </summary>
        /// <param name="id">The primary key for the event.</param>
        /// <returns>An event for the specified 'id'.</returns>
        [HttpGet("{id}")]
        public IActionResult Event(int id)
        {
            return Ok(new Event()
            {
                Id = id,
                Name = "name",
                Description = "description",
                StartOn = new DateTime(),
                EndOn = new DateTime(),
                SelfUrl = $"/data/calendar/event/{id}",
                ParentUrl = $"/data/calendar/1",
                //Criteria = new[] { new { } },
                //Tags = new[] { new { } },
                //Activities = new[] { new { } },
                AddedOn = new DateTime(),
                UpdatedOn = new DateTime()
            });
        }

        /// <summary>
        /// Returns an array of events for the calendar specified by the 'id'.
        /// </summary>
        /// <param name="id">The calendar id.</param>
        /// <returns>An array of events.</returns>
        [HttpGet("/[area]/calendar/{id}/events")]
        public IActionResult EventsForCalendar(int id)
        {
            return Ok();
        }
        #endregion
    }
}
