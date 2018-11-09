using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Fosol.Schedule.DAL.Interfaces
{
    public interface IPrincipalAccessor
    {
        #region Properties
        ClaimsPrincipal Principal { get; }
        #endregion
    }
}
