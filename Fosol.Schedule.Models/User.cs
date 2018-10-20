﻿using Fosol.Schedule.Entities;
using System;
using System.Collections.Generic;

namespace Fosol.Schedule.Models
{
    public class User : BaseModel
    {
        #region Properties
        public int Id { get; set; }

        public Guid Key { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public Gender Gender { get; set; }

        public DateTime? Birthdate { get; set; }

        public Account DefaultAccount { get; set; }

        public IList<Account> OwnedAccounts { get; set; }

        public IList<Account> Accounts { get; set; }

        public IList<Attribute> Attributes { get; set; }
        #endregion
    }
}
