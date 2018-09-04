using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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
        public int Id { get; set; }

        /// <summary>
        /// get/set - Foreign key to the account that owns this calendar.
        /// </summary>
        public int AccountId { get; set; }

        /// <summary>
        /// get/set - The account this calendar belongs to.
        /// </summary>
        public Account Account { get; set; }

        /// <summary>
        /// get/set - A unique key to identify this calendar.
        /// </summary>
        public Guid Key { get; set; }

        /// <summary>
        /// get/set - A unique name within an account that identifies this calendar.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// get/set - A description of the calendar.
        /// </summary>
        [MaxLength(2000)]
        public string Description { get; set; }
        
        /// <summary>
        /// get/set - A collection of events within this calendar.
        /// </summary>
        public ICollection<Event> Events { get; set; }

        /// <summary>
        /// get/set - A collection of participants within this calendar.
        /// </summary>
        public ICollection<Participant> Participants { get; set; }
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
        /// <param name="accountId"></param>
        /// <param name="name"></param>
        public Calendar(int accountId, string name)
        {
            if (String.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name));

            this.AccountId = accountId;
            this.Name = name;
            this.Key = Guid.NewGuid();
        }
        #endregion
    }
}
