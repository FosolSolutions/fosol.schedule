namespace Fosol.Schedule.Entities
{
    /// <summary>
    /// DataType enum, provides a way to control what type of data is referenced.
    /// </summary>
    public enum DataType
    {
        /// <summary>
        /// String - a text message.
        /// </summary>
        String = 0,

        /// <summary>
        /// Integer - a number.
        /// </summary>
        Integer = 1,

        /// <summary>
        /// Decimal - a number.
        /// </summary>
        Decimal = 2,

        /// <summary>
        /// Float - a number.
        /// </summary>
        Float = 3,

        /// <summary>
        /// Double - a number.
        /// </summary>
        Double = 4,

        /// <summary>
        /// Boolean - a true or false value.
        /// </summary>
        Boolean = 5
    }
}