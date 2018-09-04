using System;
using System.Collections.Generic;

namespace Fosol.Schedule.Entities
{
    /// <summary>
    /// Participant class, provides a way to manage participant information in the datasource.
    /// </summary>
    /// <remarks>
    /// A participant is a way to associate a person with an calendar.
    /// A participant may be owned by a user account, but it could represent a person without a user account.
    /// The goal is to provide a way to manage a participant list of people who may or may not have user accounts within the application.
    /// A participant can be managed by the owner of the calendar, or the user.
    /// </remarks>
    public class Participant
    {
        #region Properties
        /// <summary>
        /// get/set - Primary key uses IDENTITY.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// get/set - A unique key to identify this participant.
        /// </summary>
        public Guid Key { get; set; }

        /// <summary>
        /// get/set - The foreign key to the user account for this participant.
        /// </summary>
        public int? UserId { get; set; }

        /// <summary>
        /// get/set - The user account that is represented by this participant.
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// get/set - Foreign key to the calendar this participant belongs to.
        /// </summary>
        public int CalendarId { get; set; }

        /// <summary>
        /// get/set - The calendar this participant belongs to.
        /// </summary>
        public Calendar Calendar { get; set; }

        /// <summary>
        /// get - A collection of information about the participant.
        /// </summary>
        public ICollection<ParticipantInfo> Information { get; set; } = new List<ParticipantInfo>();

        public ICollection<Quality> Qualities { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a Participant object.
        /// </summary>
        public Participant()
        { }
        #endregion
    }
}