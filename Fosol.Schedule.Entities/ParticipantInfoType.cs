namespace Fosol.Schedule.Entities
{
    /// <summary>
    /// ParticipantInfoType enum, provides the participant information type options.
    /// </summary>
    public enum ParticipantInfoType
    {
        /// <summary>
        /// The participants display name.
        /// </summary>
        DisplayName = 0,

        /// <summary>
        /// A description about the participant.
        /// </summary>
        Description = 1,

        /// <summary>
        /// The participant's email address.
        /// </summary>
        Email = 2,

        /// <summary>
        /// The participant's address.
        /// </summary>
        Address = 3,

        /// <summary>
        /// The participant's phone number.
        /// </summary>
        Phone = 4,

        /// <summary>
        /// The participant's fax number.
        /// </summary>
        Fax = 5,

        /// <summary>
        /// Other information about the participant.
        /// </summary>
        Other = 6
    }
}