using Fosol.Overseer.Requesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fosol.Schedule.DAL.Requestors.Accounts
{
    public class AccountRequest : IRequest<Models.Account>
    {
        #region Properties
        public int Id { get; set; }

        public Models.Account Account { get; set; }
        #endregion
    }
}
