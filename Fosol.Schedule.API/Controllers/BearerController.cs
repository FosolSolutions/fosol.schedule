using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AspNet.Security.OAuth.Validation;
using Fosol.Schedule.API.Helpers;
using Fosol.Schedule.DAL.Interfaces;
using Microsoft.AspNetCore.Mvc;
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
		#endregion

		#region Constructors
		/// <summary>
		/// Creates a new instance of an BearerController object, and initializes it with the specified arguments.
		/// </summary>
		/// <param name="dataSource"></param>
		/// <param name="logger"></param>
		public BearerController(IDataSource dataSource, ILogger<AuthController> logger)
		{
			_dataSource = dataSource;
			_logger = logger;
		}
		#endregion

		#region Endpoints
		/// <summary>
		/// Jwt Bearer authentication endpoint.
		/// This will return the user with a JWT token.
		/// </summary>
		/// <param name="userName"></param>
		/// <param name="password"></param>
		/// <returns></returns>
		[HttpGet("authenticate"), HttpPost("authenticate")]
		public IActionResult AuthenticateUser(string userName, string password)
		{
			var user = _dataSource.Users.Get(1);
			if (user == null)
				return BadRequest();

			var identity = _dataSource.Users.CreateIdentity(user.Key.Value);
			if (identity == null)
				return Unauthorized();

			var tokenHandler = new JwtSecurityTokenHandler();
			var key = Encoding.ASCII.GetBytes("this is my custom Secret key for authnetication");
			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Audience = "https://localhost:44321",
				Issuer = "https://localhost:44375",
				Subject = identity,
				Expires = DateTime.UtcNow.AddDays(7),
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
			};
			var token = tokenHandler.CreateToken(tokenDescriptor);

			return Ok(new {
				Id = user.Id,
				Email = user.Email,
				Token = tokenHandler.WriteToken(token)
			});
		}

		//[HttpGet("/.well-known/openid-configuration")]
		//public async Task<IActionResult> Configuration()
		//{
		//	return Ok();
		//}

		[HttpGet("token")]
		public async Task<IActionResult> Token()
		{
			return Ok();
		}

		[HttpGet("user/information")]
		public async Task<IActionResult> UserInformation()
		{
			return Ok();
		}
		#endregion
	}
}