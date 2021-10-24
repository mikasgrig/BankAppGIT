using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Persistence.Models;

namespace Persistence.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private const string TableName = "transaction";
        private readonly ISqlClient _sqlClient;

        public TransactionRepository(ISqlClient sqlClient)
        {
            _sqlClient = sqlClient;
        }
        public Task<int> SaveAsync(TransactionReadModel model)
        {
            var sql = @$"INSERT INTO {TableName} (Id, AccountId, DateCreate, Debit, Credit, Description, TransactionType) 
                        VALUES (@Id, @AccountId, @DateCreate, @Debit, @Credit, @Description, @TransactionType)";
            return _sqlClient.ExecuteAsync(sql, new
            {
                model.Id,
                model.AccountId,
                model.DateCreate,
                model.Debit,
                model.Credit,
                model.Description,
                TransactionType = model.TransactionType.ToString()
            });
        }

        public Task<IEnumerable<TransactionReadModel>> GetAllAsync(Guid accountId)
        {
            var sql = $"SELECT * FROM {TableName} WHERE AccountId = @AccountId ORDER BY DateCreate DESC" ;
            return _sqlClient.QueryAsync<TransactionReadModel>(sql, new
            {
                AccountId = accountId
            });
        }
    }
}