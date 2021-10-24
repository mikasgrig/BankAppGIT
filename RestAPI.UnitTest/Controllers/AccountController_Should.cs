using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using Domain.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.DataCollection;
using Moq;
using Newtonsoft.Json.Serialization;
using Persistence.Models;
using Persistence.Repositories;
using Xunit;

namespace RestAPI.UnitTest.Controllers
{
    public class AccountController_Should
    {
        private readonly Mock<HttpContext> _httpContextMock = new Mock<HttpContext>();
        private readonly Mock<IUsersRepository> _usersRepository = new Mock<IUsersRepository>();
        private readonly Mock<ITransactionRepository> _transactionRepository = new Mock<ITransactionRepository>();
        private readonly Mock<IAccountService> _accountService = new Mock<IAccountService>();

        
        

        [Theory, AutoData]
        public async Task GetAllTodoItems_When_GetAllIsCalled(
            Guid userId,
            AccountReadModel accountReadModel)
        {
            // Arrange
            _httpContextMock.SetupGet(httpContext => httpContext.Items["userId"]).Returns(userId);   
        }
    }
}