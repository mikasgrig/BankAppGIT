using System;
using System.Threading.Tasks;
using Contract.Models.Request;
using Contract.Models.Response;
using Domain.Clients.Firebase;
using Persistence.Models;
using Persistence.Repositories;

namespace Domain.Services
{
    public class AuthService : IAuthService
    {
        private readonly IFirebaseClient _firebaseClient;
        private readonly IUsersRepository _usersRepository;
        private readonly IAccountRepository _accountRepository;
        
        public AuthService(IFirebaseClient firebaseClient, IUsersRepository usersRepository, IAccountRepository accountRepository)
        {
            _firebaseClient = firebaseClient;
            _usersRepository = usersRepository;
            _accountRepository = accountRepository;
        }
        public async Task<SignUpResponse> SignUpAsync(SignUpRequest request)
        {
            var user = await _firebaseClient.SignUpAsync(request.Email, request.Password);
            
            var userReadModel = new UserReadModel
            {
                Id = Guid.NewGuid(),
                FirebaseId = user.FirebaseId,
                Email = user.Email,
                DateCreated = DateTime.Now
            };

            await _usersRepository.SaveAsync(userReadModel);
            await _accountRepository.Save(new AccountReadModel
            {
                Id = Guid.NewGuid(),
                UserId = userReadModel.Id,
                Amount = 0,
                Name = request.AccountName,
                DateCreated = userReadModel.DateCreated
            });
            
            return new SignUpResponse
            {
                Id = userReadModel.Id,
                IdToken = user.IdToken,
                Email = userReadModel.Email,
                DateCreated = userReadModel.DateCreated
            };
        }
        public async Task<SignInResponse> SignInAsync(SignInRequest request)
        {
            var firebaseSignInResponse = await _firebaseClient.SignInAsync(request.Email, request.Password);

            var user = await _usersRepository.GetAsync(firebaseSignInResponse.FirebaseId);

            return new SignInResponse
            {
                Email = user.Email,
                IdToken = firebaseSignInResponse.IdToken
            };
        }
    }
}