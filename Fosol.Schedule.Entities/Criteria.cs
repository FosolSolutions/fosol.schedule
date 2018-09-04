namespace Fosol.Schedule.Entities
{
    public abstract class Criteria : BaseEntity
    {
        #region Properties
        public LogicalOperator LogicalOperator { get; set; }
        #endregion
    }
}