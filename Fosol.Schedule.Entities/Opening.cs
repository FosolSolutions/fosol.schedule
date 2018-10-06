using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fosol.Schedule.Entities
{
    public class Opening : BaseEntity
    {
        #region Properties
        /// <summary>
        /// get/set - The primary key uses IDENTITY.
        /// </summary>
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// get/set - The foreign key to the parent activity.
        /// </summary>
        public int ActivityId { get; set; }

        /// <summary>
        /// get/set - The parent activity.
        /// </summary>
        [ForeignKey(nameof(ActivityId))]
        public Activity Activity { get; set; }

        /// <summary>
        /// get/set - A unique key to identify this opening.
        /// </summary>
        [Required]
        public Guid Key { get; set; }

        /// <summary>
        /// get/set - A name to identify this opening.
        /// </summary>
        [Required, MaxLength(250)]
        public string Name { get; set; }

        /// <summary>
        /// get/set - A description about this opening.
        /// </summary>
        [MaxLength(2000)]
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
        public OpeningType OpeningType { get; set; }

        /// <summary>
        /// get/set - The process participants can apply for this opening.
        /// </summary>
        public ApplicationProcess ApplicationProcess { get; set; }

        /// <summary>
        /// get/set - A collection of participants.
        /// </summary>
        public ICollection<OpeningParticipant> OpeningParticipants { get; set; } = new List<OpeningParticipant>();

        /// <summary>
        /// get/set - A collection of applications.
        /// </summary>
        public ICollection<OpeningParticipantApplication> OpeningParticipantApplications { get; set; } = new List<OpeningParticipantApplication>();

        /// <summary>
        /// get/set - A collection of criteria.
        /// </summary>
        public ICollection<OpeningCriteria> OpeningCriteria { get; set; } = new List<OpeningCriteria>();
        #endregion
    }
}