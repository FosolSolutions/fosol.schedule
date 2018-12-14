using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AspNet.Security.OAuth.Validation;
using Fosol.Schedule.DAL.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace Fosol.Schedule.API.Controllers
{
	/// <summary>
	/// OauthController class, provides a way to authenticate a user.
	/// </summary>
	[Produces("application/json")]
	[Route("[controller]")]
	public class OauthController : Controller
	{
		#region Variables
		private readonly ILogger _logger;
		private readonly IDataSource _dataSource;
		#endregion

		#region Constructors
		/// <summary>
		/// Creates a new instance of an OauthController object, and initializes it with the specified arguments.
		/// </summary>
		/// <param name="dataSource"></param>
		/// <param name="logger"></param>
		public OauthController(IDataSource dataSource, ILogger<AuthController> logger)
		{
			_dataSource = dataSource;
			_logger = logger;
		}
		#endregion

		#region Endpoints
		/// <summary>
		/// Oauth authentication endpoint.
		/// This will return the user with a JWT token.
		/// </summary>
		/// <param name="email"></param>
		/// <param name="password"></param>
		/// <returns></returns>
		[HttpGet("authorize")]
		public async Task<IActionResult> Authorize(string email = "test", string password = "test")
		{
			var tokenHandler = new JwtSecurityTokenHandler();
			var key = Encoding.ASCII.GetBytes("this is my custom Secret key for authnetication");
			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(new Claim[]
				{
					new Claim(ClaimTypes.Name, email)
				}),
				Expires = DateTime.UtcNow.AddDays(7),
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
			};
			var token = tokenHandler.CreateToken(tokenDescriptor);

			var user = new {
				Id = 1,
				Email = email,
				Token = tokenHandler.WriteToken(token)
			};

			var u = new ClaimsPrincipal();
			var ticket = new AuthenticationTicket(u, new AuthenticationProperties(), OAuthValidationDefaults.AuthenticationScheme);
			return SignIn(u, ticket.Properties, ticket.AuthenticationScheme);
		}

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