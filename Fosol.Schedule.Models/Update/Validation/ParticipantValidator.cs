using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fosol.Schedule.Models.Update.Validation
{
  public class ParticipantValidator : AbstractValidator<Participant>
  {
    #region Constructors
    public ParticipantValidator()
    {
      RuleFor(m => m.Id).GreaterThanOrEqualTo(0);
    }
    #endregion
  }
}
