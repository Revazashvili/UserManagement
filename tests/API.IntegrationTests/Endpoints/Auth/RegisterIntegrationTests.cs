using API.IntegrationTests.Common;
using API.IntegrationTests.Extensions;
using Application.Common.DTOs.Auth;
using MediatR;

namespace API.IntegrationTests.Endpoints.Auth;

public class RegisterIntegrationTests : BaseClassFixture
{
    public RegisterIntegrationTests(TestingApiFactory<Program> testingApiFactory) 
        : base(testingApiFactory) { }
    
    [Fact]
    public async Task Register_ErrorWhenPassInValidData()
    {
        const string requestUri = "api/Auth/Register";
        var request = new RegisterUserRequest
        {
            Email = "test@gmail.com",
            Password = "12345678", // password must include character from a to z
            ConfirmPassword = "12345678"
        };
        var response = await Client.PostAsync<RegisterUserRequest,Unit>(requestUri, request);
        Assert.False(response.Succeeded,response.Errors.FirstOrDefault());
    }
    
    [Fact]
    public async Task Register_SuccessWhenPassValidData()
    {
        const string requestUri = "api/Auth/Register";
        var request = new RegisterUserRequest
        {
            Email = "test@gmail.com",
            Password = "yourstringpassword",
            ConfirmPassword = "yourstringpassword"
        };
        var response = await Client.PostAsync<RegisterUserRequest,Unit>(requestUri, request);
        Assert.True(response.Succeeded,response.Errors.FirstOrDefault());
    }
}