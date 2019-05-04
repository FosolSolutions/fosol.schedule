namespace Fosol.Schedule.Models.Update
{
  public class QuestionOption : BaseModel
  {
    #region Properties
    /// <summary>
    /// get/set - Primary key uses IDENTITY.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// get/set - The answer or option that will be displayed ot the participant.
    /// </summary>
    public string Value { get; set; }

    /// <summary>
    /// get/set - The order to display the options.
    /// </summary>
    public int Sequence { get; set; }
    #endregion
  }
}