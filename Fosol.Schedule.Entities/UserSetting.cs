using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fosol.Schedule.Entities
{
    /// <summary>
    /// UserSetting class, provides an object to manage user settings in the datasource.
    /// </summary>
    public class UserSetting : BaseEntity
    {
        #region Properties
        /// <summary>
        /// get/set - The primary key which is a foreign key to the user.
        /// </summary>
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int UserId { get; set; }

        /// <summary>
        /// get/set - The user this setting belongs to.
        /// </summary>
        [ForeignKey(nameof(UserId))]
        public User User { get; set; }

        /// <summary>
        /// get/set - A key to identify the setting.
        /// </summary>
        [Required, MaxLength(50)]
        public string Key { get; set; }

        /// <summary>
        /// get/set - The value of the settings.
        /// </summary>
        [Required, MaxLength(500)]
        public string Value { get; set; }

        /// <summary>
        /// get/set - The type of the value.
        /// </summary>
        [Required, MaxLength(100)]
        public string ValueType { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a UserSetting object.
        /// </summary>
        public UserSetting()
        {

        }

        /// <summary>
        /// Creates a new instance of a UserSetting object, and initializes it with the specified properties.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="type"></param>
        public UserSetting(User user, string key, string value, Type type)
        {
            if (String.IsNullOrWhiteSpace(key)) throw new ArgumentException($"Argument 'key' cannot be null, empty or whitespace.");

            this.UserId = user?.Id ?? throw new ArgumentNullException(nameof(user));
            this.User = user;
            this.Key = key;
            this.Value = value;
            this.ValueType = type?.FullName ?? throw new ArgumentNullException(nameof(type));
        }
        #endregion
    }
}
