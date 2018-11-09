using Fosol.Core.Mvc;
using Fosol.Schedule.DAL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fosol.Schedule.API.Areas.Data.Controllers
{
    /// <summary>
    /// SubscriptionController sealed class, provides API endpoints for calendar subscriptions.
    /// </summary>
    [Produces("application/json")]
    [Area("admin")]
    [Route("[area]/subscription/[controller]")]
    [Authorize]
    public sealed class SubscriptionController : ApiController
    {
        #region Variables
        private readonly IDataSource _dataSource;
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a SubscriptionController object.
        /// </summary>
        /// <param name="datasource"></param>
        public SubscriptionController(IDataSource datasource)
        {
            _dataSource = datasource;
        }
        #endregion

        #region Methods

        /// <summary>
        /// Returns a calendar subscription for the specified 'id'.
        /// </summary>
        /// <param name="id">The primary key for the subscription.</param>
        /// <returns>An subscription for the specified 'id'.</returns>
        [HttpGet("{id}")]
        public IActionResult GetSubscription(int id)
        {
            var subscription = _dataSource.Subscriptions.Get(id);
            return Ok(subscription);
        }

        /// <summary>
        /// Adds the new subscription to the datasource.
        /// </summary>
        /// <param name="subscription"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult AddSubscription([FromBody] Schedule.Models.Subscription subscription)
        {
            _dataSource.Subscriptions.Add(subscription);
            _dataSource.CommitTransaction();

            return Created(Url.RouteUrl(nameof(GetSubscription), new { subscription.Id }), subscription);
        }

        /// <summary>
        /// Updates the specified subscription in the datasource.
        /// </summary>
        /// <param name="subscription"></param>
        /// <returns></returns>
        [HttpPut]
        public IActionResult UpdateSubscription([FromBody] Schedule.Models.Subscription subscription)
        {
            _dataSource.Subscriptions.Update(subscription);
            _dataSource.CommitTransaction();

            return Ok(subscription);
        }

        /// <summary>
        /// Deletes the specified subscription from the datasource.
        /// </summary>
        /// <param name="subscription"></param>
        /// <returns></returns>
        [HttpDelete]
        public IActionResult DeleteSubscription([FromBody] Schedule.Models.Subscription subscription)
        {
            _dataSource.Subscriptions.Remove(subscription);
            _dataSource.CommitTransaction();

            return Ok();
        }
        #endregion
    }
}
