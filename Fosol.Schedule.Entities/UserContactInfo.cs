using System;
using System.Collections.Generic;
using System.Text;

namespace Fosol.Schedule.Entities
{
    /// <summary>
    /// UserContactInfo class, provides a way to manage a users contact information (email, phone, etc.) in the datasource.
    /// </summary>
    public class UserContactInfo
    {
        #region Properties
        /// <summary>
        /// get/set - Primary key and Foreign key to the user who this contact information belongs to.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// get/set - The user this contact information belongs to.
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// get/set - Primary key, a unique name to identify this contact information [Mobile|Home|Email|...]
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// get/set - The type of contact information.
        /// </summary>
        public ContactInfoType Type { get; set; }

        /// <summary>
        /// get/set - The email address, phone number of other other.
        /// </summary>
        public string Value { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instances of a UserContactInfo object.
        /// </summary>
        public UserContactInfo()
        { }

        /// <summary>
        /// Creates a new instance of a UserContactInfo object, and initializes it with the specified properties.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <param name="value"></param>
        public UserContactInfo(int userId, string name, ContactInfoType type, string value)
        {
            if (String.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name));

            if (String.IsNullOrWhiteSpace(value))
                throw new ArgumentNullException(nameof(value));

            this.UserId = userId;
            this.Name = name;
            this.Type = type;
            this.Value = value;
        }
        #endregion
    }
}
