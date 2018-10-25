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
        /// get/set - The state of the opening.
        /// </summary>
        public OpeningState State { get; set; } = OpeningState.Published;

        /// <summary>
        /// get/set - A collection of participants that have been accepted to the opening.
        /// </summary>
        public ICollection<OpeningParticipant> Participants { get; set; } = new List<OpeningParticipant>();

        /// <summary>
        /// get/set - A collection of applications.  These are participants that are apply for the opening.
        /// </summary>
        public ICollection<OpeningParticipantApplication> Applications { get; set; } = new List<OpeningParticipantApplication>();

        /// <summary>
        /// get/set - A collection of criteria.
        /// </summary>
        public ICollection<OpeningCriteria> Criteria { get; set; } = new List<OpeningCriteria>();
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a Opening object.
        /// </summary>
        public Opening()
        {

        }

        /// <summary>
        /// Creates a new instance of a Opening object, and initializes it with the specified property values.
        /// </summary>
        /// <param name="activity"></param>
        /// <param name="name"></param>
        /// <param name="minParticipants"></param>
        /// <param name="maxParticipants"></param>
        /// <param name="type"></param>
        /// <param name="process"></param>
        public Opening(Activity activity, string name, int minParticipants, int maxParticipants, OpeningType type, ApplicationProcess process, OpeningState state = OpeningState.Published)
        {
            if (String.IsNullOrWhiteSpace(name)) throw new ArgumentException($"Argument 'name' cannot be null, empty or whitespace.");
            if (minParticipants <= 0) throw new ArgumentException($"Argument 'minParticipants' must be greater than 0.");
            if (maxParticipants < minParticipants) throw new ArgumentException($"Argument 'maxParticipants' must be greater than or equal to 'minParticipants'.");

            this.ActivityId = activity?.Id ?? throw new ArgumentNullException(nameof(activity));
            this.Activity = activity;
            this.Name = name;
            this.MinParticipants = minParticipants;
            this.MaxParticipants = maxParticipants;
            this.OpeningType = type;
            this.ApplicationProcess = process;
            this.Key = Guid.NewGuid();
            this.State = state;
        }
        #endregion
    }
}