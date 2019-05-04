namespace Fosol.Schedule.Models.Create
{
  public class Criteria : BaseModel
  {
    #region Properties
    /// <summary>
    /// get/set - Primary key uses IDENTITY.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// get/set - The criteria associated with the calendar.
    /// </summary>
    public string Statement { get; set; }
    #endregion
  }
}