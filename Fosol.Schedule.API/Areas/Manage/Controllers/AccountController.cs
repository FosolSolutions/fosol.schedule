﻿using Fosol.Core.Mvc;
using Fosol.Schedule.DAL.Interfaces;
using Fosol.Schedule.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fosol.Schedule.API.Areas.Data.Controllers
{
    /// <summary>
    /// AccountController sealed class, provides API endpoints for calendar accounts.
    /// </summary>
    [Produces("application/json")]
    [Area("manage")]
    [Route("[area]/[controller]")]
    [Authorize]
    public sealed class AccountController : ApiController
    {
        #region Variables
        private readonly IDataSource _dataSource;
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a AccountController object.
        /// </summary>
        /// <param name="datasource"></param>
        public AccountController(IDataSource datasource)
        {
            _dataSource = datasource;
        }
        #endregion

        #region Methods

        /// <summary>
        /// Returns a calendar account for the specified 'id'.
        /// </summary>
        /// <param name="id">The primary key for the account.</param>
        /// <returns>An account for the specified 'id'.</returns>
        [HttpGet("{id}")]
        public IActionResult GetAccount(int id) // TODO: Should I use async?
        {
            var account = _dataSource.Accounts.Get(id);
            return Ok(account);
        }

        /// <summary>
        /// Adds the new account to the datasource.
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        [HttpPost("/[area]/[controller]")]
        public IActionResult AddAccount([FromBody] Account account)
        {
            _dataSource.Accounts.Add(account);
            _dataSource.CommitTransaction();

            return Created(Url.RouteUrl(nameof(GetAccount), new { account.Id }), account);
        }

        /// <summary>
        /// Updates the specified account in the datasource.
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        [HttpPut("/[area]/[controller]")]
        public IActionResult UpdateAccount([FromBody] Account account)
        {
            _dataSource.Accounts.Update(account);
            _dataSource.CommitTransaction();

            return Ok(account);
        }

        /// <summary>
        /// Deletes the specified account from the datasource.
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        [HttpDelete("/[area]/[controller]")]
        public IActionResult DeleteAccount([FromBody] Account account)
        {
            _dataSource.Accounts.Remove(account);
            _dataSource.CommitTransaction();

            return Ok();
        }
        #endregion
    }
}
