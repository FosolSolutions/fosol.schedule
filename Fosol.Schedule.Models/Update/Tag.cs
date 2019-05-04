namespace Fosol.Schedule.Models.Update
{
  public class Tag : BaseModel
  {
    #region Properties
    /// <summary>
    /// get/set - The tag key associated with the calendar.
    /// </summary>
    public string Key { get; set; }

    /// <summary>
    /// get/set - The tag value associated with the calendar.
    /// </summary>
    public string Value { get; set; }
    #endregion
  }
}