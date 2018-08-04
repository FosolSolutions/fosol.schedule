using System;
using System.Collections.Generic;
using System.Text;

namespace Fosol.Schedule.Entities
{
    public class CriteriaGroup : Criteria
    {
        #region Properties
        public ICollection<Criteria> Criterias { get; set; }
        #endregion
    }
}
