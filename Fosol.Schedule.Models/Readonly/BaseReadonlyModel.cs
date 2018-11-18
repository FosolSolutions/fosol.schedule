using System;

namespace Fosol.Schedule.Models.Readonly
{
	public abstract class BaseReadonlyModel
	{
		#region Properties

		/// <summary>
		/// get/set - When this entity was created.
		/// </summary>
		public DateTime AddedOn { get; set; } = DateTime.UtcNow;

		/// <summary>
		/// get/set - When this entity was updated last.
		/// </summary>
		public DateTime? UpdatedOn { get; set; }
		#endregion
	}
}