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
        public DataType DataType { get; set; }
        #endregion
    }
}