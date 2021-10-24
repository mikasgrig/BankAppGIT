using System.Threading.Tasks;
using Contract.Models;
using Contract.Models.Response;

namespace Domain.Services
{
    public interface IAccountService
    {
        Task<AccountResponse> Deposit(DepositWriteModel model);
        Task<AccountResponse> SendMoney(SendWriteModel model);
        
    }
}