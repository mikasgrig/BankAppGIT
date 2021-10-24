using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Persistence.Models;

namespace Persistence.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private const string TableName = "account";
        private readonly ISqlClient _sqlClient;

        public AccountRepository(ISqlClient sqlClient)
        {
            _sqlClient = sqlClient;
        }   
        public Task<int> Save(AccountReadModel model)
        {
            var sql = @$"INSERT INTO {TableName} (Id, UserId, Amount, Name, DateCreated) 
                        VALUES (@Id, @UserId, @Amount, @Name, @DateCreated)";
            return _sqlClient.ExecuteAsync(sql, model);
        }

        public Task<IEnumerable<AccountReadModel>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<AccountReadModel> GetAsync(Guid userid)
        {
            var sql = $"SELECT * FROM {TableName} WHERE UserId = @UserId";
            return _sqlClient.QuerySingleOrDefaultAsync<AccountReadModel>(sql, new
            {
                UserId = userid,
            });
        }

        public Task<AccountReadModel> GetAsyncById(Guid id)
        {
            var sql = $"SELECT * FROM {TableName} WHERE Id = @Id";
            return _sqlClient.QuerySingleOrDefaultAsync<AccountReadModel>(sql, new
            {
                Id = id,
            });
        }

        public Task<int> ChangeAmount(AmountWriteModel model)
        {
            var sql = $@"UPDATE {TableName}
SET Amount = @Amount
WHERE Id = @Id";
            return _sqlClient.ExecuteAsync(sql, model);
        }
    }
}