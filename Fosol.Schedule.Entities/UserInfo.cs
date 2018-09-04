using System;
using System.Collections.Generic;
using System.Text;

namespace Fosol.Schedule.Entities
{
    /// <summary>
    /// UserInfo class, provides a way to manage user information in the datasource.
    /// </summary>
    public class UserInfo : BaseEntity
    {
        #region Properties
        /// <summary>
        /// get/set - Primary key and foreign key to the user.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// get/set - A user name to display for other users to see.
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// get/set - The persons title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// get/set - The persons first name.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// get/set - The persons middle name.
        /// </summary>
        public string MiddleName { get; set; }

        /// <summary>
        /// get/set - The persons last name.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// get/set - The persons birthdate.
        /// </summary>
        public DateTime? BirthDate { get; set; }

        /// <summary>
        /// get/set - A description of the person.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// get/set - The perons gender.
        /// </summary>
        public Gender? Gender { get; set; }

        /// <summary>
        /// get/set - A collection of addresses for this user.
        /// </summary>
        public ICollection<UserAddress> Addresses { get; set; } = new List<UserAddress>();
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instances of a UserInfo object.
        /// </summary>
        public UserInfo()
        { }

        /// <summary>
        /// Creates anew instance of a UserInfo object, and initializes it with the specified properties.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="displayName"></param>
        public UserInfo(int userId, string displayName)
        {
            if (String.IsNullOrWhiteSpace(displayName))
                throw new ArgumentNullException(nameof(displayName));

            this.UserId = userId;
            this.DisplayName = displayName;
        }

        /// <summary>
        /// Creates anew instance of a UserInfo object, and initializes it with the specified properties.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        public UserInfo(int userId, string firstName, string lastName)
        {
            if (String.IsNullOrWhiteSpace(firstName))
                throw new ArgumentNullException(nameof(firstName));

            if (String.IsNullOrWhiteSpace(lastName))
                throw new ArgumentNullException(nameof(lastName));

            this.UserId = userId;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.DisplayName = $"{firstName} {lastName}";
        }
        #endregion
    }
}
