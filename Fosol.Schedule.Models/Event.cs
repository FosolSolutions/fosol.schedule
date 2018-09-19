using System;

namespace Fosol.Schedule.Models
{
    public class Event : BaseModel
    {
        #region Properties
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string SelfUrl { get; set; }

        public string ParentUrl { get; set; }

        public DateTime StartOn { get; set; }

        public DateTime EndOn { get; set; }
        #endregion
    }
}
