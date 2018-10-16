using Fosol.Schedule.Entities;
using System;
using System.Collections.Generic;

namespace Fosol.Schedule.Models
{
    public class User
    {
        #region Properties
        public int Id { get; set; }

        public Guid Key { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public Gender Gender { get; set; }

        public IList<Attribute> Attributes { get; set; }
        #endregion
    }
}
