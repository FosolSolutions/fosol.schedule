using System.Collections.Generic;

namespace Fosol.Schedule.Models.Read
{
  public class OpeningApplication : BaseModel
  {
    #region Properties
    public int OpeningId { get; set; }

    public Participant Participant { get; set; }

    public OpeningApplicationState State { get; set; }

    public IList<Answer> Answers { get; set; }
    #endregion
  }
}
