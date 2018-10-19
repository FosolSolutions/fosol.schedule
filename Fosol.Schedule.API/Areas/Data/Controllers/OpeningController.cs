using Fosol.Core.Mvc;
using Fosol.Schedule.DAL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Fosol.Schedule.API.Areas.Data.Controllers
{
    /// <summary>
    /// OpeningController sealed class, provides API endpoints for calendar event openings.
    /// </summary>
    [Produces("application/json")]
    [Area("data")]
    [Route("[area]/calendar/event/activity/[controller]")]
    public sealed class OpeningController : ApiController
    {
        #region Variables
        private readonly IDataSource _datasource;
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a OpeningController object.
        /// </summary>
        /// <param name="datasource"></param>
        public OpeningController(IDataSource datasource)
        {
            _datasource = datasource;
        }
        #endregion

        #region Methods

        /// <summary>
        /// Returns an event opening for the specified 'id'.
        /// </summary>
        /// <param name="id">The primary key for the opening.</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public IActionResult GetOpening(int id)
        {
            var opening = _datasource.Openings.Get(id);
            return Ok(opening);
        }

        /// <summary>
        /// Add the new opening to the datasource.
        /// </summary>
        /// <param name="opening"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult AddOpening([FromBody] Models.Opening opening)
        {
            _datasource.Openings.Add(opening);
            _datasource.CommitTransaction();

            return Created(Url.RouteUrl(nameof(GetOpening), new { opening.Id }), opening);
        }

        /// <summary>
        /// Update the specified opening in the datasource.
        /// </summary>
        /// <param name="opening"></param>
        /// <returns></returns>
        [HttpPut]
        public IActionResult UpdateOpening([FromBody] Models.Opening opening)
        {
            _datasource.Openings.Update(opening);
            _datasource.CommitTransaction();

            return Ok(opening);
        }

        /// <summary>
        /// Delete the specified opening from the datasource.
        /// </summary>
        /// <param name="opening"></param>
        /// <returns></returns>
        [HttpDelete]
        public IActionResult DeleteOpening([FromBody] Models.Opening opening)
        {
            _datasource.Openings.Remove(opening);
            _datasource.CommitTransaction();

            return Ok();
        }
        #endregion
    }
}
