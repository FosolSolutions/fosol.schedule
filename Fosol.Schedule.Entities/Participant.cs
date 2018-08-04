using System.Collections.Generic;

namespace Fosol.Schedule.Entities
{
    public class Participant
    {
        #region Properties
        public int Id { get; set; }
        public int UserId { get; set; }
        public int CalendarId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string Country { get; set; }
        public string AddressCode { get; set; }
        public ICollection<Quality> Qualities { get; set; }
        #endregion
    }
}