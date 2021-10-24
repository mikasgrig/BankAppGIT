using System.Threading.Tasks;
using AutoFixture.Xunit2;
using FluentAssertions;
using Moq;
using System;
using Contract.Models.Request;
using Domain.Clients.Firebase;
using Domain.Clients.Firebase.Models;
using Domain.Services;
using Persistence.Models;
using Persistence.Repositories;
using TestHelper.Attributes;
using Xunit;

namespace RestAPI.UnitTest.Controllers
{
    public class AuthController_Should
    {
     [Theory, AutoMoqData]
        public async Task SignInAsync_WithSignInRequest_ReturnsSignInResponse(
            SignInRequest signInRequest,
            FirebaseSignInResponse firebaseSignInResponse,
            UserReadModel userReadModel,
            [Frozen] Mock<IFirebaseClient> firebaseClientMock,
            [Frozen] Mock<IUsersRepository> usersRepositoryMock,
            AuthService sut)
        {
            // Arrange
            firebaseSignInResponse.Email = signInRequest.Email;

            userReadModel.FirebaseId = firebaseSignInResponse.FirebaseId;
            userReadModel.Email = firebaseSignInResponse.Email;
  
            firebaseClientMock
                .Setup(firebaseClient => firebaseClient.SignInAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(firebaseSignInResponse);
        
            usersRepositoryMock
                .Setup(userRepository => userRepository.GetAsync(firebaseSignInResponse.FirebaseId))
                .ReturnsAsync(userReadModel);
        
            // Act
            var result = await sut.SignInAsync(signInRequest);
        
            // Assert
            signInRequest.Email.Should().BeEquivalentTo(result.Email);
            firebaseSignInResponse.IdToken.Should().BeEquivalentTo(result.IdToken);
            
            //Assert.Equal(expectedResult.Username, result.Username);
            //Assert.Equal(expectedResult.Email, result.Email);
           // Assert.Equal(expectedResult.IdToken, result.IdToken);
        }

        [Theory]
        [AutoMoqData]
        public async Task SignUpAsync_With_SignUpRequest(
            SignUpRequest signUpRequest, 
            FirebaseSignUpResponse firebaseSignUpResponse,
            [Frozen] Mock<IFirebaseClient> firebaseClientMock,
            [Frozen] Mock<IUsersRepository> usersRepositoryMock,
            AuthService sut)
        {
            // Arrange
            firebaseSignUpResponse.Email = signUpRequest.Email;

            firebaseClientMock
                .Setup(firebaseClient => firebaseClient.SignUpAsync(signUpRequest.Email, signUpRequest.Password))
                .ReturnsAsync(firebaseSignUpResponse);
            
            // Act
            var result = await sut.SignUpAsync(signUpRequest);
            
            // Assert
            usersRepositoryMock.Verify(userRepository => userRepository.SaveAsync(It.Is<UserReadModel>(model => 
            model.FirebaseId.Equals(firebaseSignUpResponse.FirebaseId) &&
            model.Email.Equals(firebaseSignUpResponse.Email))), Times.Once);
            
            firebaseClientMock
                .Verify(firebaseClient => firebaseClient.SignUpAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            
            result.Id.GetType().Should().Be<Guid>();
            result.IdToken.Should().BeEquivalentTo(firebaseSignUpResponse.IdToken);
            result.Email.Should().BeEquivalentTo(firebaseSignUpResponse.Email);
            result.Email.Should().BeEquivalentTo(signUpRequest.Email);
            result.DateCreated.GetType().Should().Be<DateTime>();
        }
    }
}    
    