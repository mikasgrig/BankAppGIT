using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Persistence.Models;

namespace Persistence.Repositories
{
    public interface ITransactionRepository
    {
        Task<int> SaveAsync(TransactionReadModel model);
        
        Task<IEnumerable<TransactionReadModel>> GetAllAsync(Guid accountId);

    }
}