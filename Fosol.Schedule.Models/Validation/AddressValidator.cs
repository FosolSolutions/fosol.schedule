using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fosol.Schedule.Models.Validation
{
    public class AddressValidator : AbstractValidator<Address>
    {
        #region Constructors
        public AddressValidator()
        {
            RuleFor(m => m.Id).GreaterThanOrEqualTo(0);
        }
        #endregion
    }
}
