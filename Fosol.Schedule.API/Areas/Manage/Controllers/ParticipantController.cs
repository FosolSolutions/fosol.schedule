using Fosol.Core.Mvc;
using Fosol.Schedule.API.Helpers.Mail;
using Fosol.Schedule.DAL.Interfaces;
using Fosol.Schedule.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Fosol.Schedule.API.Areas.Manage.Controllers
{
    /// <summary>
    /// ParticipantController class, provides endpoints to manage participants.
    /// </summary>
    [Produces("application/json")]
    [Area("manage")]
    [Route("[area]/[controller]")]
    [Authorize]
    public class ParticipantController : ApiController
    {
        #region Variables
        private readonly IDataSource _dataSource;
        private readonly MailClient _mailClient;
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a ParticipantController object, and initializes it with the specified properties.
        /// </summary>
        /// <param name="datasource"></param>
        /// <param name="mailClient"></param>
        public ParticipantController(IDataSource datasource, MailClient mailClient)
        {
            _dataSource = datasource;
            _mailClient = mailClient;
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
            var participant = _dataSource.Participants.Get(id);

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
            var participant = _dataSource.Participants.Get(key);

            return Ok(participant);
        }

        /// <summary>
        /// Get the participants for the specified calendar.
        /// </summary>
        /// <param name="calendarId">The unique id of the calendar.</param>
        /// <param name="page">The page number.</param>
        /// <param name="quantity">The number of participants to return in a single request.</param>
        /// <returns></returns>
        [HttpGet("/[area]/calendar/{calendarId}/participants")]
        public IActionResult GetParticipantsForCalendar(int calendarId, [FromQuery] int page = 1, [FromQuery] int quantity = 20)
        {
            var take = quantity < 1 ? 20 : quantity;
            var skip = page > 1 ? page - 1 * take : 0;
            var participants = _dataSource.Participants.GetForCalendar(calendarId, skip, take);
            // TODO: Need a PageRequest object.
            return Ok(participants);
        }

        /// <summary>
        /// Add the specified participant to the datasource.
        /// </summary>
        /// <param name="participant"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult AddParticipant([FromBody] Participant participant)
        {
            if (this.ModelState.IsValid)
            {
                _dataSource.Participants.Add(participant);
                _dataSource.CommitTransaction();

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
        public IActionResult UpdateParticipant([FromBody] Participant participant)
        {
            if (this.ModelState.IsValid)
            {
                _dataSource.Participants.Update(participant);
                _dataSource.CommitTransaction();

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
        public IActionResult DeleteParticipant([FromBody] Participant participant)
        {
            if (this.ModelState.IsValid)
            {
                _dataSource.Participants.Remove(participant);
                _dataSource.CommitTransaction();

                return Ok(true);
            }

            return BadRequest(this.ModelState);
        }

        /// <summary>
        /// Send an invitation to the specified participant.
        /// </summary>
        /// <param name="id">The unique id of the participant.</param>
        /// <returns>True if the invitation was successfully sent.</returns>
        [HttpPut("invite/{id}")]
        public async Task<IActionResult> InviteParticipant(int id)
        {
            var participant = _dataSource.Participants.Get(id);

            if (!String.IsNullOrWhiteSpace(participant.Email))
            {
                var message = _mailClient.CreateInvitation(participant);
                await _mailClient.SendAsync(message);

                return Ok(true);
            }
            else
            {
                return BadRequest(new JsonError(System.Net.HttpStatusCode.BadRequest, $"The participant '{participant.DisplayName}' does not have an email address."));
            }
        }
        #endregion
    }
}
