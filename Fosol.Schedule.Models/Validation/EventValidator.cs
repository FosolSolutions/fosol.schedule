using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fosol.Schedule.Models.Validation
{
    public class EventValidator : AbstractValidator<Event>
    {
        #region Constructors
        public EventValidator()
        {
            RuleFor(m => m.Id).GreaterThanOrEqualTo(0);
        }
        #endregion
    }
}
