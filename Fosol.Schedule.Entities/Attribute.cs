using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fosol.Schedule.Entities
{
    /// <summary>
    /// Attribute class, provides a way to describe an attribute or qualification that a participant has.
    /// </summary>
    public class Attribute : BaseEntity
    {
        #region Properties
        /// <summary>
        /// get/set - The primary key uses IDENTITY.
        /// </summary>
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// get/set - Primary key.  A unique way to identify a attribute.
        /// </summary>
        [Required, MaxLength(100)]
        public string Key { get; set; }

        /// <summary>
        /// get/set - Primary key.  A value to specify the attribute.
        /// </summary>
        [Required, MaxLength(100)]
        public string Value { get; set; }

        /// <summary>
        /// get/set - The datatype of the attribute.
        /// </summary>
        [Required, MaxLength(200)]
        public string ValueType { get; set; }
        #endregion

        #region Constructors
        public Attribute()
        {

        }

        public Attribute(string key, string value, Type type)
        {
            if (string.IsNullOrWhiteSpace(key)) throw new ArgumentException($"Argument 'key' cannot be null, empty or whitespace.");
            if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException($"Argument 'value' cannot be null, empty or whitespace.");

            this.Key = key;
            this.Value = value;
            this.ValueType = type?.FullName ?? throw new ArgumentNullException(nameof(type)); // TODO: handle generic names
        }

        public Attribute(string key, object value)
        {
            if (string.IsNullOrWhiteSpace(key)) throw new ArgumentException($"Argument 'key' cannot be null, empty or whitespace.");
            if (value == null) throw new ArgumentException($"Argument 'value' cannot be null.");

            this.Key = key;
            this.Value = $"{value}"; // TODO: serialize.
            this.ValueType = value.GetType()?.FullName; // TODO: handle generic names
        }
        #endregion
    }
}