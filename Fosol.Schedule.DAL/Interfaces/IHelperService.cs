namespace Fosol.Schedule.DAL.Interfaces
{
    public interface IHelperService
    {
        #region Methods
        Models.Calendar AddEcclesialCalendar(string name, string description);
        #endregion
    }
}
