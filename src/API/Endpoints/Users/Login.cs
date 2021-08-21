using System.Threading;
using System.Threading.Tasks;
using API.Routes;
using Application.Commands.Users;
using Application.Common.DTOs.Users;
using Application.Common.Models;
using Ardalis.ApiEndpoints;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace API.Endpoints.Users
{
    [Route(ApiRoutes.User)]
    public class Login : BaseAsyncEndpoint
        .WithRequest<LoginUserDto>
        .WithResponse<IResponse<string>>
    {
        private readonly IMediator _mediator;

        public Login(IMediator mediator) => _mediator = mediator;
        
        [HttpPost("Login")]
        [SwaggerOperation(Description = "Signs In Provided User",
            Summary = "Sign In",
            OperationId = "User.Login",
            Tags = new []{ "User" })]
        [SwaggerResponse(200,"User Logged In successfully",typeof(IResponse<string>))]
        [SwaggerResponse(400,"User with provided email can not be found",typeof(IResponse<string>))]
        [Produces("application/json")]
        [Consumes("application/json")]
        public override async Task<ActionResult<IResponse<string>>> HandleAsync([SwaggerRequestBody("User Login Payload",Required = true)]LoginUserDto request, 
            CancellationToken cancellationToken = new())
        {
            var result = await _mediator.Send(new LoginUserCommand(request), cancellationToken);
            return result.Succeeded ? Ok(result) : BadRequest(result);
        }
    }
}