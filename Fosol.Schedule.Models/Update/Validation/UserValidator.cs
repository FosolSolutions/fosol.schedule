using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fosol.Schedule.Models.Update.Validation
{
  public class UserValidator : AbstractValidator<User>
  {
    #region Constructors
    public UserValidator()
    {
      RuleFor(m => m.Id).GreaterThanOrEqualTo(0);
    }
    #endregion
  }
}
