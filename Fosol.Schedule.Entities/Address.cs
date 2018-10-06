using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Fosol.Schedule.Entities
{
    /// <summary>
    /// Address class, provides a way to manage address information in the datasource.
    /// </summary>
    public class Address : BaseEntity
    {
        #region Properties
        /// <summary>
        /// get/set - Primary key uses IDENTITY.
        /// </summary>
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// get/set - A unique name to identify this address.
        /// </summary>
        [Required, MaxLength(100)]
        public string Name { get; set; }

        /// <summary>
        /// get/set - Whether this address is the primary address for the reference.
        /// </summary>
        public bool IsPrimary { get; set; }

        /// <summary>
        /// get/set - Address line 1.
        /// </summary>
        [Required, MaxLength(150)]
        public string Address1 { get; set; }

        /// <summary>
        /// get/set - Address line 2.
        /// </summary>
        [MaxLength(150)]
        public string Address2 { get; set; }

        /// <summary>
        /// get/set - City name.
        /// </summary>
        [Required, MaxLength(150)]
        public string City { get; set; }

        /// <summary>
        /// get/set - State or province name.
        /// </summary>
        [Required, MaxLength(150)]
        public string Province { get; set; }

        /// <summary>
        /// get/set - ZIP or postal code.
        /// </summary>
        [Required, MaxLength(20)]
        public string PostalCode { get; set; }

        /// <summary>
        /// get/set - Country name.
        /// </summary>
        [Required, MaxLength(100)]
        public string Country { get; set; }

        /// <summary>
        /// get/set - Category for this type of address.
        /// </summary>
        public ContactInfoCategory Category { get; set; }

        /// <summary>
        /// get - A collection of users who references this address.  It'll only ever be one.
        /// </summary>
        public ICollection<UserAddress> UserAddresses { get; set; } = new List<UserAddress>();

        /// <summary>
        /// get - A collection of participants who references this address.  It'll only ever be one.
        /// </summary>
        public ICollection<Participant> Participants { get; set; } = new List<Participant>();
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instances of a UserAddress object.
        /// </summary>
        public Address()
        { }

        /// <summary>
        /// Creates a new instance of a Address object, and initializes it with the specified properties.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="address"></param>
        /// <param name="city"></param>
        /// <param name="province"></param>
        /// <param name="postal"></param>
        /// <param name="country"></param>
        /// <param name="category"></param>
        public Address(User user, string address, string city, string province, string postal, string country, ContactInfoCategory category)
        {
            this.UserAddresses.Add(new UserAddress(user, this) ?? throw new ArgumentNullException(nameof(user)));
            this.Address1 = address;
            this.City = city;
            this.Province = province;
            this.PostalCode = postal;
            this.Country = country;
            this.Category = category;
        }
        #endregion
    }
}
