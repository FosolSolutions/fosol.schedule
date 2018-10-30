using Fosol.Core.Data;
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
    public class Address : ValueObject
    {
        #region Properties
        /// <summary>
        /// get/set - A unique name to identify this address.
        /// </summary>
        [MaxLength(100)]
        public string Name { get; set; }

        /// <summary>
        /// get/set - Address line 1.
        /// </summary>
        [MaxLength(150)]
        public string Address1 { get; set; }

        /// <summary>
        /// get/set - Address line 2.
        /// </summary>
        [MaxLength(150)]
        public string Address2 { get; set; }

        /// <summary>
        /// get/set - City name.
        /// </summary>
        [MaxLength(150)]
        public string City { get; set; }

        /// <summary>
        /// get/set - State or province name.
        /// </summary>
        [MaxLength(150)]
        public string Province { get; set; }

        /// <summary>
        /// get/set - ZIP or postal code.
        /// </summary>
        [MaxLength(20)]
        public string PostalCode { get; set; }

        /// <summary>
        /// get/set - Country name.
        /// </summary>
        [MaxLength(100)]
        public string Country { get; set; }
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
        /// <param name="name"></param>
        /// <param name="user"></param>
        /// <param name="address"></param>
        /// <param name="city"></param>
        /// <param name="province"></param>
        /// <param name="postal"></param>
        /// <param name="country"></param>
        public Address(string name, string address, string city, string province, string postal, string country)
        {
            this.Name = name;
            this.Address1 = address;
            this.City = city;
            this.Province = province;
            this.PostalCode = postal;
            this.Country = country;
        }

        /// <summary>
        /// The properties of this address.
        /// </summary>
        /// <returns></returns>
        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return this.Name;
            yield return this.Address1;
            yield return this.Address2;
            yield return this.City;
            yield return this.Province;
            yield return this.Country;
            yield return this.PostalCode;
        }
        #endregion

        #region Methods

        #endregion
    }
}
