using Fosol.Core.Extensions.Collection;
using Fosol.Core.Extensions.Enumerable;
using System;
using System.Collections.Generic;
using System.Linq;

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
            values.ForEach(c => this.Criteria.Add(new CriteriaValue(c)));
        }
        #endregion

        #region Methods
        /// <summary>
        /// Validates that the attribute(s) match the criteria.
        /// Currently this is a simple check; one OR must pass and all AND must pass.
        /// </summary>
        /// <param name="attributes"></param>
        /// <returns></returns>
        public override bool Validate(params Attribute[] attributes) // TODO: Handle complex grouping.
        {
            var results = new List<Tuple<LogicalOperator, bool>>();
            foreach (var criteria in this.Criteria)
            {
                var pass = new Tuple<LogicalOperator, bool>(criteria.LogicalOperator, criteria.Validate(attributes));

                switch (criteria.LogicalOperator)
                {
                    case (LogicalOperator.Or):
                        results.Add(pass);
                        continue;
                    case (LogicalOperator.And):
                        if (!pass.Item2) return false;
                        break;
                }
            }

            return results.Any(r => r.Item2);
        }

        /// <summary>
        /// Converts the criteria group into a string value.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.ToString(false);
        }

        /// <summary>
        /// Converts the criteria group into a string value and encodes it.
        /// </summary>
        /// <param name="encode"></param>
        /// <returns></returns>
        public override string ToString(bool encode)
        {
            return String.Join(';', this.Criteria.Select(c => c.ToString(encode)));
        }
        #endregion
    }
}
