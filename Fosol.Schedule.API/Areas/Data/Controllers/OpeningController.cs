using Fosol.Core.Mvc;
using Fosol.Schedule.DAL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fosol.Schedule.API.Areas.Data.Controllers
{
	/// <summary>
	/// OpeningController sealed class, provides API endpoints for calendar event openings.
	/// </summary>
	[Produces("application/json")]
	[Area("data")]
	[Route("[area]/calendar/event/activity/[controller]")]
	[Authorize]
	public sealed class OpeningController : ApiController
	{
		#region Variables
		private readonly IDataSource _dataSource;
		#endregion

		#region Constructors
		/// <summary>
		/// Creates a new instance of a OpeningController object.
		/// </summary>
		/// <param name="datasource"></param>
		public OpeningController(IDataSource datasource)
		{
			_dataSource = datasource;
		}
		#endregion

		#region Methods
		/// <summary>
		/// Returns an event opening for the specified 'id'.
		/// </summary>
		/// <param name="id">The primary key for the opening.</param>
		/// <returns></returns>
		[HttpGet("{id}")]
		public IActionResult GetOpening(int id)
		{
			var opening = _dataSource.Openings.Get(id);
			return Ok(opening);
		}

		/// <summary>
		/// Submit an application for the current participant.
		/// </summary>
		/// <param name="application"></param>
		/// <returns></returns>
		[HttpPut("apply")]
		public IActionResult Apply([FromBody] Models.OpeningApplication application)
		{
			var result = _dataSource.Openings.Apply(application);
			_dataSource.CommitTransaction();

			return Ok(result);
		}

		/// <summary>
		/// Remove an application for the current participant.
		/// </summary>
		/// <param name="opening"></param>
		/// <returns></returns>
		[HttpPut("unapply")]
		public IActionResult Unapply([FromBody] Models.Opening opening)
		{
			var result = _dataSource.Openings.Unapply(opening);
			_dataSource.CommitTransaction();

			return Ok(result);
		}
		#endregion
	}
}
