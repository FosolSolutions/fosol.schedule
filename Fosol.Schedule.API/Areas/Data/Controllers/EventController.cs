using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fosol.Schedule.API.Areas.Data.Controllers
{
    /// <summary>
    /// <typeparamref name="EventController"/> sealed class, provides API endpoints for calendar events.
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
        /// Creates a new instance of a <typeparamref name="EventController"/> object.
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
        /// <returns></returns>
        [HttpGet("{id}")]
        public IActionResult Event(int id)
        {
            return Ok(new
            {
                Id = id,
                Name = "",
                Description = "",
                StartDate = new DateTime(),
                EndDate = new DateTime(),
                SelfUrl = $"/data/calendar/event/{id}",
                ParentUrl = $"/data/calendar/1",
                Criteria = new[] { new { } },
                Tags = new[] { new { } },
                Activities = new[] { new { } },
                DateAdded = new DateTime(),
                DateUpdated = new DateTime(),
                RowVersion = ""
            });
        }
        #endregion
    }
}
