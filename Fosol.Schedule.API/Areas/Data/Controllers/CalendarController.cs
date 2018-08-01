using Fosol.Schedule.API.Areas.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fosol.Schedule.API.Areas.Data.Controllers
{
    [Produces("application/json")]
    [Area("data")]
    [Route("[area]/[controller]")]
    public class CalendarController : Controller
    {
        #region Variables
        private readonly ILogger _logger;
        #endregion

        #region Constructors
        public CalendarController()
        {
        }
        #endregion

        #region Methods
        [HttpGet("/[area]/calendars"), AllowAnonymous]
        public IActionResult Calendars()
        {
            return Ok(CreateCalendars());
        }
        #endregion


        #region temporary
        private IEnumerable<CalendarModel> CreateCalendars()
        {
            return new List<CalendarModel>()
            {
                CreateCalendar(1),
                CreateCalendar(2)
            };
        }

        private CalendarModel CreateCalendar(int id)
        {
            var calendar = new CalendarModel()
            {
                Id = id,
                Key = Guid.NewGuid(),
                Name = $"calendar {id}",
                Description = $"calendar {id}",
                Days = new List<CalendarDayModel>(365)
            };

            for (var i=0; i < 365; i++)
            {
                calendar.Days.Add(new CalendarDayModel()
                {
                    Id = i+1,
                    Date = new DateTime(DateTime.Now.Year, 1, 1).AddDays(i)
                });
            }

            return calendar;
        }
        #endregion
    }
}
