using System;

namespace Fosol.Schedule.Entities
{
    /// <summary>
    /// OpeningParticipantApplication class, provides a way to manage the many-to-many relationship between openings and participant applications.
    /// </summary>
    public class OpeningParticipantApplication
    {
        #region Properties
        /// <summary>
        /// get/set - Foreign key to the opening associated with the role.
        /// </summary>
        public int OpeningId { get; set; }

        /// <summary>
        /// get/set - The opening associated with the role.
        /// </summary>
        public Opening Opening { get; set; }

        /// <summary>
        /// get/set - Foreign key to the contact information associated with the opening.
        /// </summary>
        public int ParticipantId { get; set; }

        /// <summary>
        /// get/set - The contact information associated with the opening.
        /// </summary>
        public Participant Participant { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instances of a OpeningParticipantApplication object.
        /// </summary>
        public OpeningParticipantApplication()
        {

        }

        /// <summary>
        /// Creates a new instance of a OpeningParticipantApplication object, and initializes it with the specified properties.
        /// </summary>
        /// <param name="opening"></param>
        /// <param name="participant"></param>
        public OpeningParticipantApplication(Opening opening, Participant participant)
        {
            this.OpeningId = opening?.Id ?? throw new ArgumentNullException(nameof(opening));
            this.Opening = opening;
            this.ParticipantId = participant?.Id ?? throw new ArgumentNullException(nameof(participant));
            this.Participant = participant;
        }
        #endregion
    }
}
