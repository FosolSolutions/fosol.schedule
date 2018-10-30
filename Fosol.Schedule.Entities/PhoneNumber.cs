using Fosol.Core.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fosol.Schedule.Entities
{
    public class PhoneNumber : ValueObject
    {
        #region Properties
        /// <summary>
        /// get/set - A unique name to identify the phone number.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// get/set - The phone number.
        /// </summary>
        public string Number { get; set; }
        #endregion

        #region Constructors
        #endregion

        #region Methods
        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return this.Name;
            yield return this.Number;
        }
        #endregion
    }
}
