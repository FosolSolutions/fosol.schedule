using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fosol.Schedule.API.Areas.Data.Controllers
{
    /// <summary>
    /// ActivityController sealed class, provides API endpoints for calendar event activities.
    /// </summary>
    [Produces("application/json")]
    [Area("data")]
    [Route("[area]/calendar/[controller]")]
    public sealed class ActivityController : Controller
    {
        #region Variables
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a ActivityController object.
        /// </summary>
        public ActivityController()
        {
        }
        #endregion

        #region Methods

        /// <summary>
        /// Returns an event activity for the specified 'id'.
        /// </summary>
        /// <param name="id">The primary key for the activity.</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public IActionResult Activity(int id)
        {
            return Ok(new
            {
                Id = id,
                Name = "",
                Description = "",
                SelfUrl = $"",
                ParentUrl = $"",
                Criteria = new[] { new { } },
                Openings = new[] { new { } },
                DateAdded = new DateTime(),
                DateUpdated = new DateTime(),
                RowVersion = ""
            });
        }
        #endregion
    }
}
