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
        private readonly List<CalendarModel> _calendars;
        private readonly ILogger _logger;
        #endregion

        #region Constructors
        public CalendarController()
        {
            _calendars = CreateCalendars();
        }
        #endregion

        #region Methods
        [HttpGet("/[area]/calendars"), AllowAnonymous]
        public IActionResult Calendars()
        {
            var calendars = _calendars.Select(c => new
            {
                c.Id,
                c.Key,
                c.Name,
                c.Description,
                Url = $"/data/calendar/{c.Id}"
            }).ToArray();
            return Ok(calendars);
        }

        [HttpGet("/[area]/calendar/{id}")]
        public IActionResult Calendar(int id)
        {
            var calendar = _calendars.FirstOrDefault(c => c.Id == id);
            return calendar != null ? Ok(calendar) : (IActionResult)NoContent();
        }
        #endregion

        #region temporary
        private List<CalendarModel> CreateCalendars()
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
                Events = new List<CalendarEventModel>(365)
            };

            // Sunday Memorial
            var jan1st = new DateTime(DateTime.Now.Year, 1, 1);
            var sunday = jan1st.DayOfWeek == DayOfWeek.Sunday ? jan1st : jan1st.AddDays(7 - (int)jan1st.DayOfWeek);
            var i = 1;
            while (sunday.Year == DateTime.Now.Year)
            {
                calendar.Events.Add(new CalendarEventModel()
                {
                    Id = i++,
                    Name = "Memorial Meeting",
                    Description = "Sunday memorial meeting.",
                    StartDate = sunday.AddHours(11),
                    EndDate = sunday.AddHours(13)
                });
                sunday = sunday.AddDays(7);
            }

            return calendar;
        }
        #endregion
    }
}
