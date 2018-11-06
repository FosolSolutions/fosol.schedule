using Fosol.Core.Mvc;
using Fosol.Schedule.DAL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Fosol.Schedule.API.Areas.Data.Controllers
{
    /// <summary>
    /// ActivityController sealed class, provides API endpoints for calendar event activities.
    /// </summary>
    [Produces("application/json")]
    [Area("data")]
    [Route("[area]/calendar/event/[controller]")]
    public sealed class ActivityController : ApiController
    {
        #region Variables
        private readonly IDataSource _dataSource;
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a ActivityController object.
        /// </summary>
        /// <param name="datasource"></param>
        public ActivityController(IDataSource datasource)
        {
            _dataSource = datasource;
        }
        #endregion

        #region Methods

        /// <summary>
        /// Returns an event activity for the specified 'id'.
        /// </summary>
        /// <param name="id">The primary key for the activity.</param>
        /// <returns>The activity JSON data object from the datasource.</returns>
        [HttpGet("{id}", Name = "GetActivity")]
        public IActionResult GetActivity(int id)
        {
            var activity = _dataSource.Activities.Get(id);
            return Ok(activity);
        }

        /// <summary>
        /// Add the new activity to the datasource.
        /// </summary>
        /// <param name="activity">The activity to add to the datasource. JSON data object in the body of the request.</param>
        /// <returns>The activity that was added to the datasource.</returns>
        [HttpPost]
        public IActionResult AddActivity([FromBody] Models.Activity activity)
        {
            _dataSource.Activities.Add(activity);
            _dataSource.CommitTransaction();

            return Created(Url.RouteUrl(nameof(GetActivity), new { activity.Id }), activity);
        }

        /// <summary>
        /// Update the specified activity in the datasource.
        /// </summary>
        /// <param name="activity">The activity to update in the datasource. JSON data object in the body of the request.</param>
        /// <returns>The activity that was updated in the datasource.</returns>
        [HttpPut]
        public IActionResult UpdateActivity([FromBody] Models.Activity activity)
        {
            _dataSource.Activities.Update(activity);
            _dataSource.CommitTransaction();

            return Ok(activity);
        }

        /// <summary>
        /// Delete the specified activity from the datasource.
        /// </summary>
        /// <param name="activity">The activity to delete from the datasource. JSON data object in the body of the request.</param>
        /// <returns>true if successful, or an error JSON object.</returns>
        [HttpDelete]
        public IActionResult DeleteActivity([FromBody] Models.Activity activity)
        {
            _dataSource.Activities.Remove(activity);
            _dataSource.CommitTransaction();

            return Ok(true);
        }
        #endregion
    }
}
