using System.Collections.Generic;
using System.Threading.Tasks;
using Contract.Models.Response;
using Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Persistence.Repositories;
using System;
using System.Linq;
using Contract.Models;
using Contract.Models.Request;

namespace RestAPI.Controllers
{
    [ApiController]
    [Route("account")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IUsersRepository _usersRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IAccountService _accountService;

        public AccountController(IAccountRepository accountRepository, IUsersRepository usersRepository,
            ITransactionRepository transactionRepository, IAccountService accountService)
        {
            _accountRepository = accountRepository;
            _usersRepository = usersRepository;
            _transactionRepository = transactionRepository;
            _accountService = accountService;
        }
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<AccountResponse>> GetAccount()
        {
            var firebaseId = HttpContext.User.Claims.SingleOrDefault(claim => claim.Type == "user_id").Value;
            
            var user = await _usersRepository.GetAsync(firebaseId);
            
            if (user is null)
            {
                return NotFound($"User token does not exists!");
            }
            
            var account = await _accountRepository.GetAsync(user.Id);
            
            return Ok(account.MapToAccountResponse());
        }
        [HttpPost]
        [Authorize]
        [Route("deposit")]
        public async Task<ActionResult<AccountResponse>> Deposit(DepositRequest request)
        {
            var firebaseId = HttpContext.User.Claims.SingleOrDefault(claim => claim.Type == "user_id").Value;
            
            var user = await _usersRepository.GetAsync(firebaseId);
            
            if (user is null)
            {
                return NotFound($"User token does not exists!");
            }
            
            var account = await _accountRepository.GetAsync(user.Id);
            
            if (account is null)
            {
                return NotFound($"Account does not exists!");
            }
            
            if (account.UserId != user.Id)
            {
                return NotFound($"Wrong deposit");
            }
            
            return await _accountService.Deposit(new DepositWriteModel
            {
                Amount = request.Amount,
                AccountId = account.Id,
                AccountAmount = account.Amount
            });
        }
        [HttpPost]
        [Authorize]
        [Route("send")]
        public async Task<ActionResult<AccountResponse>> Send(SendRequest request)
        {
            var firebaseId = HttpContext.User.Claims.SingleOrDefault(claim => claim.Type == "user_id").Value;
            
            var user = await _usersRepository.GetAsync(firebaseId);
            
            if (user is null)
            {
                return NotFound($"User token does not exists!");
            }
            
            var account = await _accountRepository.GetAsync(user.Id);
            
            if (account is null)
            {
                return NotFound($"Account does not exists!");
            }

            if (account.UserId != user.Id)
            {
                return NotFound($"Wrong send");
            }

            if (account.Amount < request.Amount)
            {
                return NotFound($"Not enough money to send");
            }
            
            var receiverAccount = await _accountRepository.GetAsyncById(request.AccountId);
            
            if (receiverAccount is null)
            {
                return NotFound($"Account does not exists!");
            }
            
            return await _accountService.SendMoney(new SendWriteModel
            {
                SendAmount = request.Amount,
                SenderAccountAmount = account.Amount,
                ReceiverAccountAmount = receiverAccount.Amount,
                SenderAccountId = account.Id,
                ReceiverAccountId = receiverAccount.Id,
                ReceiverAccountName = receiverAccount.Name,
                SenderAccountName = account.Name
            });
        }
        [HttpGet]
        [Authorize]
        [Route("transaction")]
        public async Task<ActionResult<IEnumerable<TransactionResponse>>> GetTransaction()
        {
            var firebaseId = HttpContext.User.Claims.SingleOrDefault(claim => claim.Type == "user_id").Value;
            
            var user = await _usersRepository.GetAsync(firebaseId);
            
            if (user is null)
            {
                return NotFound($"User token does not exists!");
            }
            
            var account = await _accountRepository.GetAsync(user.Id);
            
            if (account.UserId != user.Id)
            {
                return NotFound($"Account does not exists!");
            }
            
            var transactions = await _transactionRepository.GetAllAsync(account.Id);
            
            return Ok(transactions.Select(model => model.MapToTransactionResponse()));
        }
    }
}