﻿using Fosol.Schedule.API.Helpers;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace Fosol.Schedule.API.Controllers
{
	/// <summary>
	/// ApiController class, provides API endpoints that describe all other endpoints in this application.
	/// </summary>
	[Produces("application/json")]
	[Route("[controller]")]
	public class ApiController : Controller
	{
		#region Methods
		/// <summary>
		/// Returns an array of all the API endpoints in this application with their documentation.
		/// </summary>
		/// <param name="name">The name of the controller.</param>
		/// <returns>An array of Endpoint.</returns>
		[HttpGet("endpoints/{name?}")]
		public IActionResult Endpoints(string name)
		{
			return Ok(ApiHelper.GetEndpoints(name));
		}

		/// <summary>
		/// Return only the endpoint with the specified name.
		/// </summary>
		/// <param name="path">The name of the controller.</param>
		/// <param name="name">The name of the endpoint.</param>
		/// <returns></returns>
		[HttpGet("endpoint/{path}/{name}")]
		public IActionResult Endpoint(string path, string name)
		{
			var result = ApiHelper.GetEndpoint(path, name);
			return result != null ? Ok(result) : BadRequest();
		}

		/// <summary>
		/// Get the model definition.
		/// </summary>
		/// <param name="name">The full name of the model.</param>
		/// <returns></returns>
		[HttpGet("model/{name}")]
		public IActionResult Model(string name)
		{
			var type = Assembly.GetAssembly(typeof(Models.Calendar)).GetType($"Fosol.Schedule.Models.{name}", false, true) ?? Assembly.GetAssembly(typeof(Entities.Calendar)).GetType($"Fosol.Schedule.Entities.{name}", false, true);

			if (type == null) return BadRequest();

			return Ok(ApiHelper.GetModel(type));
		}
		#endregion
	}
}