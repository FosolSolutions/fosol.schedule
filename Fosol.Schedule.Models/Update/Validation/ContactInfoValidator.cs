using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fosol.Schedule.Models.Update.Validation
{
  public class ContactInfoValidator : AbstractValidator<ContactInfo>
  {
    #region Constructors
    public ContactInfoValidator()
    {
      RuleFor(m => m.Id).GreaterThanOrEqualTo(0);
    }
    #endregion
  }
}
