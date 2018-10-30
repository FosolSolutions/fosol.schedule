using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fosol.Schedule.Models.Validation
{
    public class CalendarValidator : AbstractValidator<Calendar>
    {
        #region Constructors
        public CalendarValidator()
        {
            RuleFor(m => m.Id).GreaterThanOrEqualTo(0);
        }
        #endregion
    }
}
