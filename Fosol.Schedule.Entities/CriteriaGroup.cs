using Fosol.Core.Extensions.Collection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Fosol.Schedule.Entities
{
    public class CriteriaGroup : Criteria
    {
        #region Properties
        /// <summary>
        /// get/set - Primary key uses IDENTITY.
        /// </summary>
        public int Id { get; set; }

        public IList<Criteria> Criteria { get; set; } = new List<Criteria>();
        #endregion

        #region Constructors
        public CriteriaGroup()
        {

        }

        public CriteriaGroup(LogicalOperator logicalOperator, params Criteria[] criteria)
        {
            this.LogicalOperator = LogicalOperator;
            criteria.AddRange(criteria);
        }

        public CriteriaGroup(CriteriaObject criteria)
        {
            this.Id = criteria.Id;
            var values = criteria.Criteria.Split(';');
            this.Criteria.AddRange(values.Select(v => new CriteriaValue(v)).ToArray());
        }
        #endregion

        #region Methods
        public override string ToString()
        {
            return String.Join(';', this.Criteria.Select(c => c.ToString()));
        }

        public override string ToString(bool encode)
        {
            return String.Join(';', this.Criteria.Select(c => c.ToString(encode)));
        }
        #endregion
    }
}
