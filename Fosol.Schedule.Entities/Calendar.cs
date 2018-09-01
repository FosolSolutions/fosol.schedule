using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fosol.Schedule.Entities
{
    public class Calendar : BaseEntity
    {
        #region Properties
        public int Id { get; set; }
        public int AccountId { get; set; }
        public Guid Key { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<CalendarEvent> Events { get; set; }
        public ICollection<Participant> Participants { get; set; }
        #endregion

        #region Constructors
        #endregion
    }
}
