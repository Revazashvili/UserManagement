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

namespace API.Endpoints.Auth
{
    [Route(AuthRoutes.Register)]
    public class Register : BaseAsyncEndpoint
        .WithRequest<RegisterUserDto>
        .WithResponse<IResponse<string>>
    {
        private readonly IMediator _mediator;

        public Register(IMediator mediator) => _mediator = mediator;
        
        [HttpPost]
        [SwaggerOperation(Description = "Register new user and returns token",
            Summary = "Register new user",
            OperationId = "Auth.Register",
            Tags = new []{ "Auth" })]
        [SwaggerResponse(200,"User registered successfully",typeof(IResponse<string>))]
        [SwaggerResponse(400,"Some error occured during registration",typeof(IResponse<string>))]
        [Produces("application/json")]
        [Consumes("application/json")]
        public override async Task<ActionResult<IResponse<string>>> HandleAsync(
            [SwaggerRequestBody("User register payload",Required = true)]RegisterUserDto registerUserDto, 
            CancellationToken cancellationToken = new())
        {
            return Ok(await _mediator.Send(new RegisterUserCommand(registerUserDto), cancellationToken));
        }
    }
}