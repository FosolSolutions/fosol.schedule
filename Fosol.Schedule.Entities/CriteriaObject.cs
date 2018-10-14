using Fosol.Core.Extensions.Enumerable;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Fosol.Schedule.Entities
{
    public class CriteriaObject : BaseEntity
    {
        #region Properties
        [Key]
        public int Id { get; set; }

        [Required]
        public string Criteria { get; set; }

        [Required]
        public bool IsGroup { get; set; }
        #endregion

        #region Constructors
        public CriteriaObject()
        {
            this.IsGroup = false;
        }

        public CriteriaObject(LogicalOperator logicalOperator, string key, string value, DataType type)
        {
            this.IsGroup = false;
            // TODO: encode key and value.
            this.Criteria = $"{logicalOperator},{key},{value},{type}";
        }

        public CriteriaObject(CriteriaValue criteria)
        {
            this.IsGroup = false;
            this.Id = criteria.Id;
            this.Criteria = criteria.ToString();
        }

        public CriteriaObject(CriteriaGroup criteria)
        {
            this.IsGroup = true;
            this.Id = criteria.Id;
            this.Criteria = criteria.ToString(true);
        }

        public CriteriaObject(params CriteriaValue[] criteria)
        {
            this.IsGroup = true;
            this.Criteria = String.Join(";", criteria.Select(c => c.ToString(true)));
        }
        #endregion

        #region Methods
        public static explicit operator Criteria(CriteriaObject criteria)
        {
            return criteria.IsGroup ? new CriteriaGroup(criteria) : new CriteriaValue(criteria) as Criteria;
        }
        #endregion
    }
}
