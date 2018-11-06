using Fosol.Core.Extensions.Principals;
using Fosol.Core.Mvc;
using Fosol.Schedule.DAL.Interfaces;
using Fosol.Schedule.Models;
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
        private readonly IDataSource _dataSource;
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a OpeningController object.
        /// </summary>
        /// <param name="datasource"></param>
        public OpeningController(IDataSource datasource)
        {
            _dataSource = datasource;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Returns an event opening for the specified 'id'.
        /// </summary>
        /// <param name="id">The primary key for the opening.</param>
        /// <returns></returns>
        [HttpGet("{id}", Name = "GetOpening")]
        public IActionResult GetOpening(int id)
        {
            var opening = _dataSource.Openings.Get(id);
            return Ok(opening);
        }

        /// <summary>
        /// Add the new opening to the datasource.
        /// </summary>
        /// <param name="opening"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult AddOpening([FromBody] Opening opening)
        {
            _dataSource.Openings.Add(opening);
            _dataSource.CommitTransaction();

            return Created(Url.RouteUrl(nameof(GetOpening), new { opening.Id }), opening);
        }

        /// <summary>
        /// Update the specified opening in the datasource.
        /// </summary>
        /// <param name="opening"></param>
        /// <returns></returns>
        [HttpPut]
        public IActionResult UpdateOpening([FromBody] Opening opening)
        {
            _dataSource.Openings.Update(opening);
            _dataSource.CommitTransaction();

            return Ok(opening);
        }

        /// <summary>
        /// Delete the specified opening from the datasource.
        /// </summary>
        /// <param name="opening"></param>
        /// <returns></returns>
        [HttpDelete]
        public IActionResult DeleteOpening([FromBody] Opening opening)
        {
            _dataSource.Openings.Remove(opening);
            _dataSource.CommitTransaction();

            return Ok();
        }

        /// <summary>
        /// Submit an application for the current participant.
        /// </summary>
        /// <param name="opening"></param>
        /// <returns></returns>
        [HttpPut("apply")]
        public IActionResult Apply([FromBody] Opening opening)
        {
            var result = _dataSource.Openings.Apply(opening);
            _dataSource.CommitTransaction();

            return Ok(result);
        }

        /// <summary>
        /// Remove an application for the current participant.
        /// </summary>
        /// <param name="opening"></param>
        /// <returns></returns>
        [HttpPut("unapply")]
        public IActionResult Unapply([FromBody] Opening opening)
        {
            var result = _dataSource.Openings.Unapply(opening);
            _dataSource.CommitTransaction();

            return Ok(result);
        }
        #endregion
    }
}
