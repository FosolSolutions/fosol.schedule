using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fosol.Schedule.Entities
{
    /// <summary>
    /// UserAddress class, provides a way to manage the many-to-many relationship between users and addresses.
    /// </summary>
    public class UserAddress
    {
        #region Properties
        /// <summary>
        /// get/set - Foreign key to the user associated with the address.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// get/set - The user associated with the address.
        /// </summary>
        [ForeignKey(nameof(UserId))]
        public User User { get; set; }

        /// <summary>
        /// get/set - Foreign key to the address associated with the user.
        /// </summary>
        public int AddressId { get; set; }

        /// <summary>
        /// get/set - The address associated with the user.
        /// </summary>
        [ForeignKey(nameof(AddressId))]
        public Address Address { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instances of a UserAddress object.
        /// </summary>
        public UserAddress()
        {

        }

        /// <summary>
        /// Creates a new instance of a UserAddress object, and initializes it with the specified properties.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="address"></param>
        public UserAddress(User user, Address address)
        {
            this.UserId = user?.Id ?? throw new ArgumentNullException(nameof(user));
            this.AddressId = address?.Id ?? throw new ArgumentNullException(nameof(address));
        }
        #endregion
    }
}
