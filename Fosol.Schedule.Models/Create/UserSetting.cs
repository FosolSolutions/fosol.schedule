namespace Fosol.Schedule.Models.Create
{
  public class UserSetting : BaseModel
  {
    #region Properties
    /// <summary>
    /// get/set - A key to identify the setting.
    /// </summary>
    public string Key { get; set; }

    /// <summary>
    /// get/set - The value of the settings.
    /// </summary>
    public string Value { get; set; }

    /// <summary>
    /// get/set - The type of the value.
    /// </summary>
    public string ValueType { get; set; }
    #endregion
  }
}