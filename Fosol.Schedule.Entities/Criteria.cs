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
        public abstract bool Validate(params Attribute[] attributes);

        /// <summary>
        /// Converts the criteria expression into a string.
        /// </summary>
        /// <param name="encode"></param>
        /// <returns></returns>
        public virtual string ToString(bool encode)
        {
            return base.ToString();
        }
        #endregion
    }
}