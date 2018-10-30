using System;
using System.Linq;

namespace Fosol.Schedule.Entities
{
    /// <summary>
    /// CriteriaObject class, provides an object within the datasource that represents a criteria.
    /// </summary>
    public class CriteriaObject : BaseEntity
    {
        #region Properties
        /// <summary>
        /// get/set - Primary key uses IDENTITY.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// get/set - The criteria statement.
        /// </summary>
        public string Criteria { get; set; }

        /// <summary>
        /// get/set - Whether the criteria is a group of criteria.
        /// </summary>
        public bool IsGroup { get; set; }
        #endregion

        #region Constructors
        public CriteriaObject()
        {
            this.IsGroup = false;
        }

        public CriteriaObject(string key, string value, Type type) : this(LogicalOperator.And, key, value, type)
        {
        }

        public CriteriaObject(LogicalOperator logicalOperator, string key, string value, Type type)
        {
            this.IsGroup = false;
            this.Criteria = new CriteriaValue(logicalOperator, key, value, type).ToString(true);
        }

        public CriteriaObject(string key, object value) : this(LogicalOperator.And, key, value)
        {
        }

        public CriteriaObject(LogicalOperator logicalOperator, string key, object value)
        {
            this.IsGroup = false;
            this.Criteria = new CriteriaValue(logicalOperator, key, value).ToString(true);
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

        public CriteriaObject(params Criteria[] criteria)
        {
            this.IsGroup = true;
            this.Criteria = String.Join(";", criteria.Select(c => c.ToString(true)));
        }

        public CriteriaObject(CriteriaObject criteria)
        {
            this.IsGroup = criteria.IsGroup;
            this.Criteria = criteria.ToString(true);
        }
        #endregion

        #region Methods
        public static explicit operator Criteria(CriteriaObject criteria)
        {
            return criteria.IsGroup ? new CriteriaGroup(criteria) : new CriteriaValue(criteria) as Criteria;
        }

        public override string ToString()
        {
            return this.ToString(false);
        }

        public string ToString(bool encode)
        {
            var criteria = this.IsGroup ? new CriteriaGroup(this) : new CriteriaValue(this) as Criteria;
            return criteria?.ToString(encode);
        }
        #endregion
    }
}
