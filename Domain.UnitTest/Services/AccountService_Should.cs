using System;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using Contract.Models;
using Domain.Services;
using FluentAssertions;
using Moq;
using Persistence.Models;
using Persistence.Repositories;
using RestAPI;
using TestHelper.Attributes;
using Xunit;

namespace Domain.UnitTest.Services
{
    public class AccountService_Should
    {
        [Theory, AutoMoqData]
        public async Task Deposit_ReturnsAccountResponse(
            DepositWriteModel model,
            AccountReadModel accountReadModel,
            [Frozen] Mock<IAccountRepository> accountRepositoryMock,
            [Frozen] Mock<ITransactionRepository> transactionRepositoryMock,
            AccountService sut)
        {
            // Arrange
            accountReadModel.Id = model.AccountId;
            
            accountRepositoryMock.Setup(mock => mock.GetAsyncById(It.IsAny<Guid>()))
                .ReturnsAsync(accountReadModel);
            
            // Act
            var result = await sut.Deposit(model);
            
            // Assert
           transactionRepositoryMock
               .Verify(mock => mock.SaveAsync(It.Is<TransactionReadModel>(readmodel => 
                   readmodel.AccountId.Equals(model.AccountId) &&
                   readmodel.Debit.Equals(model.Amount))), Times.Once);
           accountRepositoryMock.Verify(mock => mock.GetAsyncById(It.IsAny<Guid>()), Times.Once);

           accountReadModel.Id.Should().Be(model.AccountId);
        }

        [Theory, AutoMoqData]
        public async Task SendMoney_ReturnsAccountResponse(SendWriteModel model,
            AccountReadModel accountReadModel,
            [Frozen] Mock<IAccountRepository> accountRepositoryMock,
            [Frozen] Mock<ITransactionRepository> transactionRepositoryMock,
            AccountService sut)
        {
            // Arrange
            accountRepositoryMock.Setup(mock => mock.GetAsyncById(model.SenderAccountId))
                .ReturnsAsync(accountReadModel);
            // Act
            var result = await sut.SendMoney(model);
            // Assert
            transactionRepositoryMock.Verify(mock => mock.SaveAsync(It.IsAny<TransactionReadModel>()), Times.Exactly(2));
            accountRepositoryMock.Verify(mock => mock
                .ChangeAmount(It.IsAny<AmountWriteModel>()), Times.Exactly(2));
            accountRepositoryMock.Verify(mock => mock
                .GetAsyncById(It.IsAny<Guid>()), Times.Once);
            result.Should().BeEquivalentTo(accountReadModel.MapToAccountResponse());
        }
    }
}