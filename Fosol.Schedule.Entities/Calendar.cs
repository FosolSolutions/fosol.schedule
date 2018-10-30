using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fosol.Schedule.Entities
{
    /// <summary>
    /// Calendar class, provides a way to manage a calendar in the datasource.  A calendar contains events.
    /// </summary>
    public class Calendar : BaseEntity
    {
        #region Properties
        /// <summary>
        /// get/set - Primary key uses IDENTITY.
        /// </summary>
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// get/set - Foreign key to the account that owns this calendar.
        /// </summary>
        public int AccountId { get; set; }

        /// <summary>
        /// get/set - The account this calendar belongs to.
        /// </summary>
        [ForeignKey(nameof(AccountId))]
        public Account Account { get; set; }

        /// <summary>
        /// get/set - A unique key to identify this calendar.
        /// </summary>
        [Required]
        public Guid Key { get; set; }

        /// <summary>
        /// get/set - A unique name within an account that identifies this calendar.
        /// </summary>
        [Required, MaxLength(100)]
        public string Name { get; set; }

        /// <summary>
        /// get/set - A description of the calendar.
        /// </summary>
        [MaxLength(2000)]
        public string Description { get; set; }

        /// <summary>
        /// get/set - The state of the calendar.
        /// </summary>
        public CalendarState State { get; set; } = CalendarState.Published;

        /// <summary>
        /// get - A collection of events within this calendar.
        /// </summary>
        public ICollection<Event> Events { get; private set; } = new List<Event>();

        /// <summary>
        /// get - A collection of participants within this calendar.
        /// </summary>
        public ICollection<Participant> Participants { get; private set; } = new List<Participant>();

        /// <summary>
        /// get - A collection of attributes for this calendar.
        /// </summary>
        public ICollection<CalendarAttribute> Attributes { get; private set; } = new List<CalendarAttribute>();

        /// <summary>
        /// get - A collection of criteria for this calendar.
        /// </summary>
        public ICollection<CalendarCriteria> Criteria { get; private set; } = new List<CalendarCriteria>();
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a <typeparamref name="Calendar"/> object.
        /// </summary>
        public Calendar()
        {

        }

        /// <summary>
        /// Creates a new instance of a <typeparamref name="Calendar"/> object, and initializes it with the specified properties.
        /// </summary>
        /// <param name="account"></param>
        /// <param name="name"></param>
        /// <param name="state"></param>
        public Calendar(Account account, string name, CalendarState state = CalendarState.Published)
        {
            if (String.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name));

            this.AccountId = account?.Id ?? throw new ArgumentNullException(nameof(account));
            this.Account = account;
            this.Name = name;
            this.Key = Guid.NewGuid();
            this.State = state;
        }
        #endregion
    }
}
