using System.Threading.Tasks;
using Contract.Models.Request;
using Contract.Models.Response;
using Domain.Exceptions;
using Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace RestAPI.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        [HttpPost]
        [Route("signUp")]
        public async Task<ActionResult<SignUpResponse>> SignUp(SignUpRequest request)
        {
            try
            {
                var response = await _authService.SignUpAsync(request);

                return response;
            }
            catch (FirebaseException e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpPost]
        [Route("signIn")]
        public async Task<ActionResult<SignInResponse>> SignIn(SignInRequest request)
        {
            return await _authService.SignInAsync(request);
        }
    }
}