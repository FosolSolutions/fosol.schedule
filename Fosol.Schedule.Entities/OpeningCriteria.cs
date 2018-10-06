using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fosol.Schedule.Entities
{
    /// <summary>
    /// OpeningCriteria class, provides a way to manage the many-to-many relationship between openings and criteria.
    /// </summary>
    public class OpeningCriteria
    {
        #region Properties
        /// <summary>
        /// get/set - Foreign key to the opening.
        /// </summary>
        public int OpeningId { get; set; }

        /// <summary>
        /// get/set - The opening associated with the criteria.
        /// </summary>
        [ForeignKey(nameof(OpeningId))]
        public Opening Opening { get; set; }

        /// <summary>
        /// get/set - Foreign key to the criteria.
        /// </summary>
        public int CriteriaId { get; set; }

        /// <summary>
        /// get/set - The criteria associated with the opening.
        /// </summary>
        [ForeignKey(nameof(CriteriaId))]
        public Criteria Criteria { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a OpeningCriteria object.
        /// </summary>
        public OpeningCriteria()
        {

        }

        /// <summary>
        /// Creates a new instance of a OpeningCriteria object, and initializes it with the specified properties.
        /// </summary>
        /// <param name="opening"></param>
        /// <param name="criteria"></param>
        public OpeningCriteria(Opening opening, Criteria criteria)
        {
            this.OpeningId = opening?.Id ?? throw new ArgumentNullException(nameof(opening));
            this.Opening = opening;
            this.CriteriaId = criteria?.Id ?? throw new ArgumentNullException(nameof(criteria));
            this.Criteria = criteria;
        }
        #endregion
    }
}
