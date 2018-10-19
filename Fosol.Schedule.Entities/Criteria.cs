namespace Fosol.Schedule.Entities
{
    /// <summary>
    /// Criteria class, provides a way to manage criteria in the datasource.
    /// </summary>
    public abstract class Criteria
    {
        #region Properties
        public LogicalOperator LogicalOperator { get; set; }
        #endregion

        #region Methods
        public virtual string ToString(bool encode)
        {
            return base.ToString();
        }
        #endregion
    }
}