using System;
using System.Collections.Generic;

namespace Fosol.Schedule.Entities
{
    /// <summary>
    /// Tag class, provides a way to manage a list of tags in the datasource.
    /// </summary>
    public class Tag : BaseEntity
    {
        #region Properties
        /// <summary>
        /// get/set - Primary key, unique category type [Category|...]
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// get/set - Primary key, unique category value (i.e. Sports)
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// get - A collection of events this tag is associated with.
        /// </summary>
        public ICollection<Event> Events { get; private set; } = new List<Event>();
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a Tag object.
        /// </summary>
        public Tag()
        { }

        /// <summary>
        /// Creates a new instance of a Tag object, and initalizes it with the specified properties.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public Tag(string key, string value)
        {
            if (String.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException(nameof(key));

            if (String.IsNullOrWhiteSpace(value))
                throw new ArgumentNullException(nameof(value));

            this.Key = key;
            this.Value = value;
        }
        #endregion
    }
}