using Fosol.Core.Mvc;
using Fosol.Schedule.DAL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Fosol.Schedule.API.Areas.Data.Controllers
{
    /// <summary>
    /// AccountController sealed class, provides API endpoints for calendar accounts.
    /// </summary>
    [Produces("application/json")]
    [Area("manage")]
    [Route("[area]/[controller]")]
    public sealed class AccountController : ApiController
    {
        #region Variables
        private readonly IDataSource _datasource;
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a AccountController object.
        /// </summary>
        /// <param name="datasource"></param>
        public AccountController(IDataSource datasource)
        {
            _datasource = datasource;
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
            var account = _datasource.Accounts.Get(id);
            return Ok(account);
        }

        /// <summary>
        /// Adds the new account to the datasource.
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult AddAccount([FromBody] Models.Account account)
        {
            _datasource.Accounts.Add(account);
            _datasource.CommitTransaction();

            return Created(Url.RouteUrl(nameof(GetAccount), new { account.Id }), account);
        }

        /// <summary>
        /// Updates the specified account in the datasource.
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        [HttpPut]
        public IActionResult UpdateAccount([FromBody] Models.Account account)
        {
            _datasource.Accounts.Update(account);
            _datasource.CommitTransaction();

            return Ok(account);
        }

        /// <summary>
        /// Deletes the specified account from the datasource.
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        [HttpDelete]
        public IActionResult DeleteAccount([FromBody] Models.Account account)
        {
            _datasource.Accounts.Remove(account);
            _datasource.CommitTransaction();

            return Ok();
        }
        #endregion
    }
}
