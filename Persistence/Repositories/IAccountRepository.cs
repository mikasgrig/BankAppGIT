using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Persistence.Models;

namespace Persistence.Repositories
{
    public interface IAccountRepository
    {
        Task<int> Save(AccountReadModel model);
        
        Task<AccountReadModel> GetAsync(Guid userid);
 
        Task<AccountReadModel> GetAsyncById(Guid id);
        Task<int> ChangeAmount(AmountWriteModel model);

    }
}