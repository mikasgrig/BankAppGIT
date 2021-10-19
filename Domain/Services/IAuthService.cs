using System.Threading.Tasks;
using Contract.Models.Request;
using Contract.Models.Response;

namespace Domain.Services
{
    public interface IAuthService
    {
        Task<SignUpResponse> SignUpAsync(SignUpRequest request);

        Task<SignInResponse> SignInAsync(SignInRequest request);
    }
}