﻿using Fosol.Core.Mvc;
using Fosol.Schedule.DAL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Fosol.Schedule.API.Areas.Data.Controllers
{
    /// <summary>
    /// UserController sealed class, provides API endpoints for calendar users.
    /// </summary>
    [Produces("application/json")]
    [Area("manage")]
    [Route("[area]/[controller]")]
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
        [HttpPost]
        public IActionResult AddUser([FromBody] Models.User user)
        {
            _dataSource.Users.Add(user);
            _dataSource.CommitTransaction();

            return Created(Url.RouteUrl(nameof(GetUser), new { user.Id }), user);
        }

        /// <summary>
        /// Updates the specified user in the datasource.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPut]
        public IActionResult UpdateUser([FromBody] Models.User user)
        {
            _dataSource.Users.Update(user);
            _dataSource.CommitTransaction();

            return Ok(user);
        }

        /// <summary>
        /// Deletes the specified user from the datasource.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpDelete]
        public IActionResult DeleteUser([FromBody] Models.User user)
        {
            _dataSource.Users.Remove(user);
            _dataSource.CommitTransaction();

            return Ok();
        }
        #endregion
    }
}
