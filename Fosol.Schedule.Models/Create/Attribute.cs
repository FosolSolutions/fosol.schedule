namespace Fosol.Schedule.Models.Create
{
  public class Attribute : CalendarAttribute
  {
    #region Properties
    /// <summary>
    /// get/set - Foreign key to the calendar which these attributes belong to.
    /// </summary>
    public int CalendarId { get; set; }
    #endregion
  }
}