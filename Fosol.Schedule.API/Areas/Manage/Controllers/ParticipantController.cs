using Fosol.Core.Mvc;
using Fosol.Schedule.DAL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Fosol.Schedule.API.Areas.Manage.Controllers
{
    /// <summary>
    /// ParticipantController class, provides endpoints to manage participants.
    /// </summary>
    [Produces("application/json")]
    [Area("manage")]
    [Route("[area]/[controller]")]
    public class ParticipantController : ApiController
    {
        #region Variables
        private readonly IDataSource _datasource;
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a ParticipantController object, and initializes it with the specified properties.
        /// </summary>
        /// <param name="datasource"></param>
        public ParticipantController(IDataSource datasource)
        {
            _datasource = datasource;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Get the participant with the specified 'id'.
        /// </summary>
        /// <param name="id">The unique key to identify the participant.</param>
        /// <returns></returns>
        [HttpGet("{id}", Name = nameof(GetParticipant))]
        public IActionResult GetParticipant(int id)
        {
            var participant = _datasource.Participants.Get(id);

            return Ok(participant);
        }

        /// <summary>
        /// Get the participant with the specified 'key'.
        /// </summary>
        /// <param name="key">The unique key to identify the participant.</param>
        /// <returns></returns>
        [HttpGet("{key}")]
        public IActionResult GetParticipant(Guid key)
        {
            var participant = _datasource.Participants.Get(key);

            return Ok(participant);
        }

        /// <summary>
        /// Get the participants for the specified calendar.
        /// </summary>
        /// <param name="calendarId"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet("/[area]/calendar/{calendarId}/participants")]
        public IActionResult GetParticipantsForCalendar(int calendarId, [FromQuery] int page)
        {
            var take = 20;
            var skip = page > 1 ? page - 1 * take : 0;
            var participants = _datasource.Participants.GetForCalendar(calendarId, skip, take);

            return Ok(participants);
        }

        /// <summary>
        /// Add the specified participant to the datasource.
        /// </summary>
        /// <param name="participant"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult AddParticipant([FromBody] Models.Participant participant)
        {
            if (this.ModelState.IsValid)
            {
                _datasource.Participants.Add(participant);
                _datasource.CommitTransaction();

                return Created(Url.RouteUrl(nameof(GetParticipant), new { participant.Id }), participant);
            }

            return BadRequest(this.ModelState);
        }

        /// <summary>
        /// Update the specified participant in the datasource.
        /// </summary>
        /// <param name="participant"></param>
        /// <returns></returns>
        [HttpPut]
        public IActionResult UpdateParticipant([FromBody] Models.Participant participant)
        {
            if (this.ModelState.IsValid)
            {
                _datasource.Participants.Update(participant);
                _datasource.CommitTransaction();

                return Ok(participant);
            }

            return BadRequest(this.ModelState);
        }

        /// <summary>
        /// Delete the specified participant from the datasource.
        /// </summary>
        /// <param name="participant"></param>
        /// <returns></returns>
        [HttpDelete]
        public IActionResult DeleteParticipant([FromBody] Models.Participant participant)
        {
            if (this.ModelState.IsValid)
            {
                _datasource.Participants.Remove(participant);
                _datasource.CommitTransaction();

                return Ok(true);
            }

            return BadRequest(this.ModelState);
        }
        #endregion
    }
}
