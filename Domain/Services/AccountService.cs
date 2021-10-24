using System;
using System.Threading.Tasks;
using Contract.Enum;
using Contract.Models;
using Contract.Models.Response;
using Persistence.Models;
using Persistence.Repositories;
using RestAPI;

namespace Domain.Services
{
    public class AccountService : IAccountService
    {
       private readonly IAccountRepository _accountRepository;
        private readonly ITransactionRepository _transactionRepository;

        public AccountService(IAccountRepository accountRepository, ITransactionRepository transactionRepository)
        {
            _accountRepository = accountRepository;
            _transactionRepository = transactionRepository;
        }
        
        public async Task<AccountResponse> Deposit(DepositWriteModel model)
        {
            var amountUpdate = model.AccountAmount + model.Amount;
            await Task.WhenAll(_accountRepository.ChangeAmount(new AmountWriteModel
            {
                Id = model.AccountId,
                Amount = amountUpdate
            }));
            var asd = await _transactionRepository.SaveAsync(new TransactionReadModel
            {
                Id = Guid.NewGuid(),
                AccountId = model.AccountId,
                Debit = model.Amount,
                Credit = 0,
                DateCreate = DateTime.Now,
                Description = "Deposit",
                TransactionType = TransactionType.Debit
            });
            var accountUpdate = await _accountRepository.GetAsyncById(model.AccountId);
            
            return accountUpdate.MapToAccountResponse();
        }
        public async Task<AccountResponse> SendMoney(SendWriteModel model)
        {
            var amountUpdate = model.SenderAccountAmount - model.SendAmount;
            var receiverAccountAmount = model.ReceiverAccountAmount + model.SendAmount;
            await Task.WhenAll(
                _accountRepository.ChangeAmount(new AmountWriteModel
                {
                    Id = model.ReceiverAccountId,
                    Amount = receiverAccountAmount
                }),
                _accountRepository.ChangeAmount(new AmountWriteModel
                {
                    Id = model.SenderAccountId,
                    Amount = amountUpdate
                }),
                _transactionRepository.SaveAsync(new TransactionReadModel
                {
                    Id = Guid.NewGuid(),
                    AccountId = model.SenderAccountId,
                    Debit = 0,
                    Credit = model.SendAmount,
                    DateCreate = DateTime.Now,
                    Description = $"{model.SenderAccountName} send to {model.ReceiverAccountName} {model.SendAmount} EURO",
                    TransactionType = TransactionType.Credit
                }),
                _transactionRepository.SaveAsync(new TransactionReadModel
                {
                    Id = Guid.NewGuid(),
                    AccountId = model.ReceiverAccountId,
                    Debit = model.SendAmount,
                    Credit = 0,
                    DateCreate = DateTime.Now,
                    Description = $"{model.SenderAccountName} send to {model.ReceiverAccountName} {model.SendAmount} EURO",
                    TransactionType = TransactionType.Debit
                }));
            var accountUpdate = await _accountRepository.GetAsyncById(model.SenderAccountId);
            
            return accountUpdate.MapToAccountResponse();
        }
    }
}