using System;

namespace Fosol.Schedule.Models.Update
{
  public class UserInfo : BaseModel
  {
    #region Properties
    /// <summary>
    /// get/set - The persons title.
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// get/set - The persons first name.
    /// </summary>
    public string FirstName { get; set; }

    /// <summary>
    /// get/set - The persons middle name.
    /// </summary>
    public string MiddleName { get; set; }

    /// <summary>
    /// get/set - The persons last name.
    /// </summary>
    public string LastName { get; set; }

    /// <summary>
    /// get/set - The persons birthdate.
    /// </summary>
    public DateTime? Birthdate { get; set; }

    /// <summary>
    /// get/set - A description of the person.
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// get/set - The perons gender.
    /// </summary>
    public Gender? Gender { get; set; }

    /// <summary>
    /// get/set - The user's home address.
    /// </summary>
    public Address HomeAddress { get; set; }

    /// <summary>
    /// get/set - The users' work address.
    /// </summary>
    public Address WorkAddress { get; set; }

    /// <summary>
    /// get/set - The participants home phone.
    /// </summary>
    public string HomePhone { get; set; }

    /// <summary>
    /// get/set - The participants mobile phone.
    /// </summary>
    public string MobilePhone { get; set; }

    /// <summary>
    /// get/set - The participants work phone.
    /// </summary>
    public string WorkPhone { get; set; }
    #endregion
  }
}