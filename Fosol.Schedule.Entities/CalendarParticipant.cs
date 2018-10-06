using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fosol.Schedule.Entities
{
    /// <summary>
    /// CalendarParticipant class, provides a way to manage the many-to-many relationship between calendars and participants.
    /// </summary>
    public class CalendarParticipant
    {
        #region Properties
        /// <summary>
        /// get/set - Foreign key to the calendar associated with the participant.
        /// </summary>
        public int CalendarId { get; set; }

        /// <summary>
        /// get/set - The calendar associated with the participant.
        /// </summary>
        [ForeignKey(nameof(CalendarId))]
        public Calendar Calendar { get; set; }

        /// <summary>
        /// get/set - Foreign key to the participant associated with the calendar.
        /// </summary>
        public int ParticipantId { get; set; }

        /// <summary>
        /// get/set - The participant associated with the calendar.
        /// </summary>
        [ForeignKey(nameof(ParticipantId))]
        public Participant Participant { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instances of a CalendarParticipant object.
        /// </summary>
        public CalendarParticipant()
        {

        }

        /// <summary>
        /// Creates a new instance of a CalendarParticipant object, and initializes it with the specified properties.
        /// </summary>
        /// <param name="calendar"></param>
        /// <param name="participant"></param>
        public CalendarParticipant(Calendar calendar, Participant participant)
        {
            this.CalendarId = calendar?.Id ?? throw new ArgumentNullException(nameof(calendar));
            this.Calendar = calendar;
            this.ParticipantId = participant?.Id ?? throw new ArgumentNullException(nameof(participant));
            this.Participant = participant;
        }
        #endregion
    }
}
