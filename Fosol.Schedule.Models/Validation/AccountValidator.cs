using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fosol.Schedule.Models.Validation
{
    public class AccountValidator : AbstractValidator<Account>
    {
        #region Constructors
        public AccountValidator()
        {
            RuleFor(m => m.Id).GreaterThanOrEqualTo(0);
        }
        #endregion
    }
}
