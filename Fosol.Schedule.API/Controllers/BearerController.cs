using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Fosol.Core.Exceptions;
using Fosol.Schedule.API.Helpers;
using Fosol.Schedule.DAL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace Fosol.Schedule.API.Controllers
{
  /// <summary>
  /// BearerController class, provides a way to authenticate a user.
  /// </summary>
  [Produces("application/json")]
  [Route("[controller]")]
  public class BearerController : Controller
  {
    #region Variables
    private readonly ILogger _logger;
    private readonly IDataSource _dataSource;
    private readonly IConfiguration _configuration;
    #endregion

    #region Constructors
    /// <summary>
    /// Creates a new instance of an BearerController object, and initializes it with the specified arguments.
    /// </summary>
    /// <param name="dataSource"></param>
    /// <param name="configuration"></param>
    /// <param name="logger"></param>
    public BearerController(IDataSource dataSource, IConfiguration configuration, ILogger<AuthController> logger)
    {
      _dataSource = dataSource;
      _configuration = configuration;
      _logger = logger;
    }
    #endregion

    #region Endpoints
    /// <summary>
    /// Jwt Bearer authentication endpoint.
    /// This will return the user with a JWT token.
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPost("authenticate")]
    public IActionResult AuthenticateUser([FromBody] Models.Read.User model)
    {
      try
      {
        var userId = _dataSource.Users.Verify(model.Email);
        var user = _dataSource.Users.Get(userId);
        var identity = _dataSource.Users.CreateIdentity(user.Key.Value);

        return Ok(new
        {
          user.Id,
          user.Email,
          Token = GenerateToken(identity)
        });
      }
      catch (NoContentException)
      {
        return Unauthorized();
      }
    }

    /// <summary>
    /// Jwt Bearer authentication endpoint.
    /// This will return the participant a JWT token.
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    [HttpPost("authenticate/{key}")]
    public IActionResult AuthenticateParticipant(Guid key)
    {
      try
      {
        var participant = _dataSource.Participants.Get(key);
        var identity = _dataSource.Participants.CreateIdentity(participant.Key.Value);

        return Ok(new
        {
          participant.Id,
          participant.Email,
          Token = GenerateToken(identity)
        });
      }
      catch (NoContentException)
      {
        return Unauthorized();
      }
    }
    #endregion

    #region Methods
    private string GenerateToken(ClaimsIdentity identity)
    {
      var tokenHandler = new JwtSecurityTokenHandler();
      var key = Encoding.ASCII.GetBytes(_configuration["Authentication:CoEvent:SecretKey"]);
      var tokenDescriptor = new SecurityTokenDescriptor
      {
        Audience = _configuration["Authentication:CoEvent:Audience"],
        Issuer = _configuration["Authentication:CoEvent:ClaimsIssuer"],
        Subject = identity,
        Expires = DateTime.UtcNow.AddDays(7),
        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
      };
      var token = tokenHandler.CreateToken(tokenDescriptor);

      return tokenHandler.WriteToken(token);
    }
    #endregion
  }
}