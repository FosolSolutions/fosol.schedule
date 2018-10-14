using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fosol.Schedule.Entities
{
    public class CriteriaValue : Criteria
    {
        #region Propeties
        /// <summary>
        /// get/set - Primary key uses IDENTITY.
        /// </summary>
        public int Id { get; set; }

        public string Key { get; set; }
        public string Value { get; set; }
        public DataType DataType { get; set; }
        #endregion

        #region Constructors
        public CriteriaValue()
        {

        }

        public CriteriaValue(LogicalOperator logicalOperator, string key, string value, DataType type)
        {
            this.LogicalOperator = LogicalOperator;
            this.Key = key;
            this.Value = value;
            this.DataType = type;
        }

        public CriteriaValue(CriteriaObject criteria) : this(criteria.Criteria)
        {
            this.Id = criteria.Id;
        }

        public CriteriaValue(string criteria)
        {
            var values = criteria.Split(',');
            this.LogicalOperator = Enum.Parse<LogicalOperator>(values[0]);
            this.Key = values[1]; // TODO: decode.
            this.Value = values[2]; // TODO: decode.
            this.DataType = Enum.Parse<DataType>(values[3]);
        }
        #endregion

        #region Methods
        public override string ToString()
        {
            return $"{this.LogicalOperator},{this.Key},{this.Value},{this.DataType}";
        }

        public override string ToString(bool encode)
        {
            // TODO: encode key, value.
            return encode ? $"{this.LogicalOperator},{this.Key},{this.Value},{this.DataType}" : this.ToString();
        }
        #endregion
    }
}