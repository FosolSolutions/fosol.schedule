﻿using System;
using System.Collections.Generic;

namespace Fosol.Schedule.Models
{
    public class Activity : BaseModel
    {
        #region Properties
        public int Id { get; set; }

        public Guid Key { get; set; }

        public string Name { get; set; }

        public int EventId { get; set; }

        public string Description { get; set; }

        public string SelfUrl { get; set; }

        public DateTime StartOn { get; set; }

        public DateTime EndOn { get; set; }
        #endregion
    }
}
