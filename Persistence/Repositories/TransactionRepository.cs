using System.Collections.Generic;
using System.Threading.Tasks;
using Persistence.Models;

namespace Persistence.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        public Task<int> SaveAsync(TransactionReadModel model)
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<TransactionReadModel>> GetAllAsync()
        {
            throw new System.NotImplementedException();
        }
    }
}