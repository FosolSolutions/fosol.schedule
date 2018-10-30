﻿using Fosol.Core.Data;
using System.Collections.Generic;

namespace Fosol.Schedule.Entities.ValueObjects
{
    public class EmailAddress : ValueObject
    {
        #region Properties
        /// <summary>
        /// get/set - The phone number.
        /// </summary>
        public string Address { get; set; }
        #endregion

        #region Constructors
        public EmailAddress(string email)
        {
            this.Address = email;
        }
        #endregion

        #region Methods
        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return this.Address;
        }
        #endregion
    }
}
