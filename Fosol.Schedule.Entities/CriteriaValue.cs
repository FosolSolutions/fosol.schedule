namespace Fosol.Schedule.Entities
{
    public class CriteriaValue : Criteria
    {
        #region Propeties
        public int Id { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public DataType DataType { get; set; }
        #endregion
    }
}