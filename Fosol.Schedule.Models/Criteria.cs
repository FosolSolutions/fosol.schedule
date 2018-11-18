using System;
using System.Collections.Generic;
using System.Text;

namespace Fosol.Schedule.Models
{
	public class Criteria
	{
		#region Properties
		/// <summary>
		/// get/set - Primary key uses IDENTITY.
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// get/set - A collection of criteria conditions.
		/// </summary>
		public IList<CriteriaValue> Conditions { get; set; }
		#endregion
	}
}
