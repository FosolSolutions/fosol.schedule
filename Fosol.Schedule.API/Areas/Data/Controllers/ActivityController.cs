using Fosol.Core.Mvc;
using Fosol.Schedule.DAL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Fosol.Schedule.API.Areas.Data.Controllers
{
    /// <summary>
    /// ActivityController sealed class, provides API endpoints for calendar event activities.
    /// </summary>
    [Produces("application/json")]
    [Area("data")]
    [Route("[area]/calendar/[controller]")]
    public sealed class ActivityController : ApiController
    {
        #region Variables
        private readonly IDataSource _datasource;
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a ActivityController object.
        /// </summary>
        /// <param name="datasource"></param>
        public ActivityController(IDataSource datasource)
        {
            _datasource = datasource;
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
            var activity = _datasource.Activities.Get(id);
            return Ok(activity);
        }
        #endregion
    }
}
