using Fosol.Core.Mvc;
using Fosol.Core.Mvc.Filters;
using Fosol.Schedule.DAL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fosol.Schedule.API.Areas.Manage.Controllers
{
  /// <summary>
  /// OpeningController sealed class, provides API endpoints for calendar event openings.
  /// </summary>
  [Produces("application/json")]
  [Area("manage")]
  [Route("[area]/calendar/event/activity/[controller]")]
  [Authorize]
  [ValidateModelFilter]
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
    [HttpGet("{id}", Name = "GetOpening")]
    public IActionResult GetOpening(int id)
    {
      var opening = _dataSource.Openings.Get(id);
      return Ok(opening);
    }

    /// <summary>
    /// Add the new opening to the datasource.
    /// </summary>
    /// <param name="opening"></param>
    /// <returns></returns>
    [HttpPost("/[area]/[controller]")]
    public IActionResult AddOpening([FromBody] Models.Create.Opening opening)
    {
      var result = _dataSource.Openings.Add(opening);
      _dataSource.CommitTransaction();

      return Created(Url.RouteUrl(nameof(GetOpening), new { result.Id }), result);
    }

    /// <summary>
    /// Update the specified opening in the datasource.
    /// </summary>
    /// <param name="opening"></param>
    /// <returns></returns>
    [HttpPut("/[area]/[controller]")]
    public IActionResult UpdateOpening([FromBody] Models.Update.Opening opening)
    {
      var result = _dataSource.Openings.Update(opening);
      _dataSource.CommitTransaction();

      return Ok(result);
    }

    /// <summary>
    /// Delete the specified opening from the datasource.
    /// </summary>
    /// <param name="opening"></param>
    /// <returns></returns>
    [HttpDelete("/[area]/[controller]")]
    public IActionResult DeleteOpening([FromBody] Models.Delete.Opening opening)
    {
      _dataSource.Openings.Remove(opening);
      _dataSource.CommitTransaction();

      return Ok();
    }
    #endregion
  }
}
