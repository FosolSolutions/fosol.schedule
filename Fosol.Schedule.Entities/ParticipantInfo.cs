using System;

namespace Fosol.Schedule.Entities
{
    /// <summary>
    /// ParticipantInfo class, provides a way to manage participant information in the datasource.
    /// </summary>
    public class ParticipantInfo
    {
        #region Properties
        /// <summary>
        /// get/set - Primary key and foreign key to the participant.
        /// </summary>
        public int ParticipantId { get; set; }

        /// <summary>
        /// get/set - The participant this information is related to.
        /// </summary>
        public Participant Participant { get; set; }

        /// <summary>
        /// get/set - Primay key, a unique name to identify this information.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// get/set - The type of information this represents [DisplayName|Description|Email|Phone|...]
        /// </summary>
        public ParticipantInfoType Type { get; set; }

        /// <summary>
        /// get/set - The participant information.
        /// </summary>
        public string Value { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a ParticipantInfo object.
        /// </summary>
        public ParticipantInfo()
        { }

        /// <summary>
        /// Creates a new instance of a ParticipantInfo object, and initializes it with the specified properties.
        /// </summary>
        /// <param name="participantId"></param>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public ParticipantInfo(int participantId, ParticipantInfoType type, string name, string value)
        {
            if (String.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name));

            if (String.IsNullOrWhiteSpace(value))
                throw new ArgumentNullException(nameof(value));

            this.ParticipantId = participantId;
            this.Type = type;
            this.Name = name;
            this.Value = value;
        }
        #endregion
    }
}