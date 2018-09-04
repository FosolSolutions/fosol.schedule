using System;
using System.Collections.Generic;
using System.Text;

namespace Fosol.Schedule.Entities
{
    /// <summary>
    /// UserAddress class, provides a way to manage user address information in the datasource.
    /// </summary>
    public class UserAddress : BaseEntity
    {
        #region Properties
        /// <summary>
        /// get/set - Primary key and Foreign key to the user this address belongs to.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// get/set - The user this address belongs to.
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// get/set - Primary key, a unique name to identify this address.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// get/set - Whether this address is the primary address for the user.
        /// </summary>
        public bool IsPrimary { get; set; }

        /// <summary>
        /// get/set - Address line 1.
        /// </summary>
        public string Address1 { get; set; }

        /// <summary>
        /// get/set - Address line 2.
        /// </summary>
        public string Address2 { get; set; }

        /// <summary>
        /// get/set - City name.
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// get/set - State or province name.
        /// </summary>
        public string Province { get; set; }

        /// <summary>
        /// get/set - ZIP or postal code.
        /// </summary>
        public string PostalCode { get; set; }

        /// <summary>
        /// get/set - Country name.
        /// </summary>
        public string Country { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instances of a UserAddress object.
        /// </summary>
        public UserAddress()
        { }

        /// <summary>
        /// Creates a new instance of a UserAddress object, and initializes it with the specified properties.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="address"></param>
        /// <param name="city"></param>
        /// <param name="province"></param>
        /// <param name="postal"></param>
        /// <param name="country"></param>
        public UserAddress(int userId, string address, string city, string province, string postal, string country)
        {
            this.UserId = userId;
            this.Address1 = address;
            this.City = city;
            this.Province = province;
            this.PostalCode = postal;
            this.Country = country;
        }
        #endregion
    }
}
