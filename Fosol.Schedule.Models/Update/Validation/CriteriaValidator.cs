using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fosol.Schedule.Models.Update.Validation
{
  public class CriteriaValidator : AbstractValidator<Criteria>
  {
    #region Constructors
    public CriteriaValidator()
    {
      RuleFor(m => m.Id).GreaterThanOrEqualTo(0);
    }
    #endregion
  }
}
