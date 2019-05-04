namespace Fosol.Schedule.Models.Create
{
  public class QuestionOption : BaseModel
  {
    #region Properties
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