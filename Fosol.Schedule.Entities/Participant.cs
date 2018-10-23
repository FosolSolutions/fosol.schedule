using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
    public class Participant : BaseEntity
    {
        #region Properties
        /// <summary>
        /// get/set - Primary key uses IDENTITY.
        /// </summary>
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// get/set - A unique key to identify this participant.
        /// </summary>
        [Required]
        public Guid Key { get; set; }

        /// <summary>
        /// get/set - The state of this participant.
        /// </summary>
        public ParticipantState State { get; set; }

        /// <summary>
        /// get/set - The foreign key to the user account for this participant.
        /// </summary>
        public int? UserId { get; set; }

        /// <summary>
        /// get/set - The user account that is represented by this participant.
        /// </summary>
        [ForeignKey(nameof(UserId))]
        public User User { get; set; }

        /// <summary>
        /// get/set - Foreign key to the calendar this participant belongs to.
        /// </summary>
        public int CalendarId { get; set; }

        /// <summary>
        /// get/set - The calendar this participant belongs to.
        /// </summary>
        [ForeignKey(nameof(CalendarId))]
        public Calendar Calendar { get; set; }

        /// <summary>
        /// get/set - A participants name to display for others to see.  This should be unique within each Calendar.
        /// </summary>
        [Required, MaxLength(100)]
        public string DisplayName { get; set; }

        /// <summary>
        /// get/set - An email address that identifies this participant.
        /// </summary>
        [MaxLength(250)]
        public string Email { get; set; }

        /// <summary>
        /// get/set - The persons title.
        /// </summary>
        [MaxLength(100)]
        public string Title { get; set; }

        /// <summary>
        /// get/set - The persons first name.
        /// </summary>
        [Required, MaxLength(100)]
        public string FirstName { get; set; }

        /// <summary>
        /// get/set - The persons middle name.
        /// </summary>
        [MaxLength(100)]
        public string MiddleName { get; set; }

        /// <summary>
        /// get/set - The persons last name.
        /// </summary>
        [Required, MaxLength(100)]
        public string LastName { get; set; }

        /// <summary>
        /// get/set - The participants gender.
        /// </summary>
        public Gender? Gender { get; set; }

        /// <summary>
        /// get/set - The participants birthdate.
        /// </summary>
        public DateTime? Birthdate { get; set; }

        /// <summary>
        /// get - A collection of contact information about the participant.
        /// </summary>
        public ICollection<ParticipantContactInfo> ContactInfo { get; set; } = new List<ParticipantContactInfo>();

        /// <summary>
        /// get - A collection of addresses for the participant.
        /// </summary>
        public ICollection<ParticipantAddress> Addresses { get; set; } = new List<ParticipantAddress>();

        /// <summary>
        /// get - A collection of attributes for the participant.
        /// </summary>
        public ICollection<ParticipantAttribute> Attributes { get; set; } = new List<ParticipantAttribute>();
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a Participant object.
        /// </summary>
        public Participant()
        { }

        /// <summary>
        /// Craetes a new instance of a Participant object, and initizes it with the specified arguments.
        /// </summary>
        /// <param name="calendar"></param>
        /// <param name="user"></param>
        public Participant(Calendar calendar, User user)
        {
            this.CalendarId = calendar?.Id ?? throw new ArgumentNullException(nameof(calendar));
            this.Calendar = calendar;
            this.UserId = user?.Id ?? throw new ArgumentNullException(nameof(user));
            this.User = user;
            this.FirstName = user.Info?.FirstName;
            this.MiddleName = user.Info?.MiddleName;
            this.LastName = user.Info?.LastName;
            this.DisplayName = $"{user.Info?.FirstName} {user.Info?.LastName}"; // TODO: Configure default display name options.
            this.Title = user.Info?.Title;
            this.Gender = user.Info?.Gender;
            this.Birthdate = user.Info?.Birthdate;
            this.Key = user.Key;
            this.Email = user.Email;
        }

        /// <summary>
        /// Craetes a new instance of a Participant object, and initizes it with the specified arguments.
        /// </summary>
        /// <param name="calendar"></param>
        /// <param name="displayName"></param>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="email"></param>
        public Participant(Calendar calendar, string displayName, string firstName = null, string lastName = null, string email = null)
        {
            if (String.IsNullOrWhiteSpace(displayName))
                throw new ArgumentException($"The argument '{nameof(displayName)}' is required and cannot be null or empty.");

            this.CalendarId = calendar?.Id ?? throw new ArgumentNullException(nameof(calendar));
            this.Calendar = calendar;
            this.DisplayName = displayName;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Key = Guid.NewGuid();
            this.Email = email;
        }
        #endregion
    }
}