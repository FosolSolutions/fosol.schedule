using Fosol.Overseer.Requesting;
using Fosol.Schedule.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Fosol.Schedule.DAL.Requestors.Accounts
{
    public class GetAccount : IRequestor<AccountRequest, Models.Account>
    {
        #region Variables
        private readonly IDataSource _datasource;
        #endregion

        #region Properties
        public int Id { get; set; }
        #endregion

        #region Constructors
        public GetAccount(IDataSource datasource)
        {
            _datasource = datasource;
        }
        #endregion

        #region Methods
        public Task<Models.Account> Execute(AccountRequest request, CancellationToken cancellationToken)
        {
            return Task.Run(() => _datasource.Accounts.Get(request.Id));
        }
        #endregion
    }
}
