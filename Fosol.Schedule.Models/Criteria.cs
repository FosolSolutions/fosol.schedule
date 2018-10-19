using System;
using System.Collections.Generic;
using System.Text;

namespace Fosol.Schedule.Models
{
    public class Criteria
    {
        #region Properties
        public int Id { get; set; }

        public string Criterion { get; set; }

        public bool IsGroup { get; set; }
        #endregion
    }
}
