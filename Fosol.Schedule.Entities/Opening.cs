using System;
using System.Collections.Generic;

namespace Fosol.Schedule.Entities
{
    public class Opening
    {
        #region Properties
        public int Id { get; set; }
        public int ActivityId { get; set; }
        public Guid Key { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int MinParticipants { get; set; }
        public int MaxParticipants { get; set; }
        public OpeningType OpeningType { get; set; }
        public ApplicationProcess ApplicationProcess { get; set; }
        public ICollection<Participant> Participants { get; set; }
        public ICollection<Participant> Applications { get; set; }
        public ICollection<Criteria> Criterias { get; set; }
        #endregion
    }
}