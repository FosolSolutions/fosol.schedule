using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fosol.Schedule.Entities
{
    /// <summary>
    /// UserParticipant class, provides a way to manage the many-to-many relationship between users and participants.
    /// </summary>
    public class UserParticipant
    {
        #region Properties
        /// <summary>
        /// get/set - Foreign key to the user associated with the role.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// get/set - The user associated with the role.
        /// </summary>
        [ForeignKey(nameof(UserId))]
        public User User { get; set; }

        /// <summary>
        /// get/set - Foreign key to the contact information associated with the user.
        /// </summary>
        public int ParticipantId { get; set; }

        /// <summary>
        /// get/set - The contact information associated with the user.
        /// </summary>
        [ForeignKey(nameof(ParticipantId))]
        public Participant Participant { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instances of a UserParticipant object.
        /// </summary>
        public UserParticipant()
        {

        }

        /// <summary>
        /// Creates a new instance of a UserParticipant object, and initializes it with the specified properties.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="participant"></param>
        public UserParticipant(User user, Participant participant)
        {
            this.UserId = user?.Id ?? throw new ArgumentNullException(nameof(user));
            this.User = user;
            this.ParticipantId = participant?.Id ?? throw new ArgumentNullException(nameof(participant));
            this.Participant = participant;
        }
        #endregion
    }
}
