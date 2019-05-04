using Fosol.Core.Mvc;
using Fosol.Core.Mvc.Filters;
using Fosol.Schedule.DAL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fosol.Schedule.API.Areas.Data.Controllers
{
  /// <summary>
  /// UserController sealed class, provides API endpoints for calendar users.
  /// </summary>
  [Produces("application/json")]
  [Area("manage")]
  [Route("[area]/[controller]")]
  [Authorize]
  [ValidateModelFilter]
  public sealed class UserController : ApiController
  {
    #region Variables
    private readonly IDataSource _dataSource;
    #endregion

    #region Constructors
    /// <summary>
    /// Creates a new instance of a UserController object.
    /// </summary>
    /// <param name="datasource"></param>
    public UserController(IDataSource datasource)
    {
      _dataSource = datasource;
    }
    #endregion

    #region Methods

    /// <summary>
    /// Returns a calendar user for the specified 'id'.
    /// </summary>
    /// <param name="id">The primary key for the user.</param>
    /// <returns>An user for the specified 'id'.</returns>
    [HttpGet("{id}")]
    public IActionResult GetUser(int id)
    {
      var user = _dataSource.Users.Get(id);
      return Ok(user);
    }

    /// <summary>
    /// Adds the new user to the datasource.
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    [HttpPost("/[area]/[controller]")]
    public IActionResult AddUser([FromBody] Models.Create.User user)
    {
      var result = _dataSource.Users.Add(user);
      _dataSource.CommitTransaction();

      return Created(Url.RouteUrl(nameof(GetUser), new { result.Id }), result);
    }

    /// <summary>
    /// Updates the specified user in the datasource.
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    [HttpPut("/[area]/[controller]")]
    public IActionResult UpdateUser([FromBody] Models.Update.User user)
    {
      var result = _dataSource.Users.Update(user);
      _dataSource.CommitTransaction();

      return Ok(result);
    }

    /// <summary>
    /// Deletes the specified user from the datasource.
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    [HttpDelete("/[area]/[controller]")]
    public IActionResult DeleteUser([FromBody] Models.Delete.User user)
    {
      _dataSource.Users.Remove(user);
      _dataSource.CommitTransaction();

      return Ok();
    }
    #endregion
  }
}
