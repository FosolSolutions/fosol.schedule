﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Fosol.Schedule.Models
{
    public class Calendar : BaseModel
    {
        #region Properties
        public int Id { get; set; }

        public Guid Key { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string SelfUrl { get; set; }

        public IEnumerable<Event> Events { get; set; }
        #endregion
    }
}
