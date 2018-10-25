using System;
using System.Collections.Generic;

namespace Fosol.Schedule.Models
{
    public class Opening : BaseModel
    {
        #region Properties
        public int Id { get; set; }

        public int ActivityId { get; set; }

        public Guid Key { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        /// <summary>
        /// get/set - The minimum number of participants required for this opening.
        /// </summary>
        public int MinParticipants { get; set; }

        /// <summary>
        /// get/set - The maximum number of participants allowed in this opening.
        /// </summary>
        public int MaxParticipants { get; set; }

        /// <summary>
        /// get/set - The type of opening.
        /// </summary>
        public Entities.OpeningType OpeningType { get; set; }

        /// <summary>
        /// get/set - The process participants can apply for this opening.
        /// </summary>
        public Entities.ApplicationProcess ApplicationProcess { get; set; }

        public Entities.OpeningState State { get; set; }

        public IEnumerable<Participant> Participants { get; set; }

        public IEnumerable<Participant> Applications { get; set; }

        public IEnumerable<Criteria> Criteria { get; set; }
        #endregion
    }
}
