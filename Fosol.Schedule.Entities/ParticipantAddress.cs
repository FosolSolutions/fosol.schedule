using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fosol.Schedule.Entities
{
    /// <summary>
    /// ParticipantAddress class, provides a way to manage the many-to-many relationship between participants and addresses.
    /// </summary>
    public class ParticipantAddress
    {
        #region Properties
        /// <summary>
        /// get/set - Foreign key to the participant associated with the address.
        /// </summary>
        public int ParticipantId { get; set; }

        /// <summary>
        /// get/set - The participant associated with the address.
        /// </summary>
        [ForeignKey(nameof(ParticipantId))]
        public Participant Participant { get; set; }

        /// <summary>
        /// get/set - Foreign key to the address associated with the participant.
        /// </summary>
        public int AddressId { get; set; }

        /// <summary>
        /// get/set - The address associated with the participant.
        /// </summary>
        [ForeignKey(nameof(AddressId))]
        public Address Address { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instances of a ParticipantAddress object.
        /// </summary>
        public ParticipantAddress()
        {

        }

        /// <summary>
        /// Creates a new instance of a ParticipantAddress object, and initializes it with the specified properties.
        /// </summary>
        /// <param name="participant"></param>
        /// <param name="address"></param>
        public ParticipantAddress(Participant participant, Address address)
        {
            this.ParticipantId = participant?.Id ?? throw new ArgumentNullException(nameof(participant));
            this.AddressId = address?.Id ?? throw new ArgumentNullException(nameof(address));
        }
        #endregion
    }
}
