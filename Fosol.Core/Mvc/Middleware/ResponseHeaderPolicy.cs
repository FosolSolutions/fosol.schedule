using System;
using System.Collections.Generic;
using System.Text;

namespace Fosol.Core.Mvc.Middleware
{
    public class ResponseHeaderPolicy
    {
        #region Properties
        public IDictionary<string, string> SetHeaders { get; } = new Dictionary<string, string>();
        public ISet<string> RemoveHeaders { get; } = new HashSet<string>();
        #endregion
    }
}
