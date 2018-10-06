using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fosol.Schedule.Entities
{
    /// <summary>
    /// ContactInfo class, provides a way to manage a users contact information (email, phone, etc.) in the datasource.
    /// </summary>
    public class ContactInfo : BaseEntity
    {
        #region Properties
        /// <summary>
        /// get/set - Primary key uses IDENTITY
        /// </summary>
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// get/set - A unique name to identify this contact information.
        /// </summary>
        [Required, MaxLength(100)]
        public string Name { get; set; }

        /// <summary>
        /// get/set - The type of contact information.
        /// </summary>
        public ContactInfoType Type { get; set; }

        /// <summary>
        /// get/set - Categorizes the contact information.
        /// </summary>
        public ContactInfoCategory Category { get; set; }

        /// <summary>
        /// get/set - The email address, phone number of other other.
        /// </summary>
        [Required, MaxLength(250)]
        public string Value { get; set; }

        /// <summary>
        /// get - A collection of users who own this contact information.  It'll only ever be one.
        /// </summary>
        public ICollection<UserContactInfo> UserContactInfos { get; set; } = new List<UserContactInfo>();

        /// <summary>
        /// get - A collection of participants who references this information.  It'll only ever be one.
        /// </summary>
        public ICollection<ParticipantContactInfo> ParticipantContactInfos { get; set; } = new List<ParticipantContactInfo>();
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instances of a ContactInfo object.
        /// </summary>
        public ContactInfo()
        { }

        /// <summary>
        /// Creates a new instance of a ContactInfo object, and initializes it with the specified properties.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <param name="category"></param>
        /// <param name="value"></param>
        public ContactInfo(User user, string name, ContactInfoType type, ContactInfoCategory category, string value)
        {
            if (String.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name));

            if (String.IsNullOrWhiteSpace(value))
                throw new ArgumentNullException(nameof(value));

            this.UserContactInfos.Add(new UserContactInfo(user, this) ?? throw new ArgumentNullException(nameof(user)));
            this.Name = name;
            this.Type = type;
            this.Category = category;
            this.Value = value;
        }
        #endregion
    }
}
