using Fosol.Overseer.Requesting;
using Fosol.Schedule.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Fosol.Schedule.DAL.Requestors.Accounts
{
    public class AccountRequestor : IRequestor<AccountRequest, Models.Account>
    {
        #region Variables
        private readonly DataSource _datasource;
        #endregion

        #region Properties
        public int Id { get; set; }
        #endregion

        #region Constructors
        public AccountRequestor(IDataSource datasource)
        {
            _datasource = datasource as DataSource;
        }
        #endregion

        #region Methods
        public Task<Models.Account> Execute(AccountRequest request, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                throw new Exception("test");
                return new Models.Account();
            });
        }

        public Task<Models.Account> Get(AccountRequest request, CancellationToken cancellationToken)
        {
            return Task.Run(() => _datasource.Accounts.Get(request.Id));
        }

        public Task<Models.Account> Update(AccountRequest request, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                _datasource.Accounts.Update(request.Account);
                _datasource.CommitTransaction();
                return request.Account;
            });
        }
        #endregion
    }
}
