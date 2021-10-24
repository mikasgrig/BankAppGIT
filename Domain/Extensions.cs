using Contract.Enum;
using Contract.Models.Response;
using Persistence.Models;

namespace RestAPI
{
    public static class Extensions
    {
        public static AccountResponse MapToAccountResponse(this AccountReadModel model)
        {
            return new AccountResponse
            {
                Id = model.Id,
                UserId = model.UserId,
                Amount = model.Amount,
                Name = model.Name,
                DateCreate = model.DateCreated
            };
        }
        public static TransactionResponse MapToTransactionResponse(this TransactionReadModel model)
        {
            return new TransactionResponse
            {
                Id = model.Id,
                AccountId = model.AccountId,
                DateCreate = model.DateCreate,
                Debit = model.Debit,
                Credit = model.Credit,
                Description = model.Description,
                TransactionType = model.TransactionType
            };
        }
    }
}