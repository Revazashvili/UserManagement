using System.Threading;
using System.Threading.Tasks;
using API.Routes;
using Application.Commands.Auth;
using Application.Common.DTOs.Auth;
using Application.Common.Models;
using Ardalis.ApiEndpoints;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace API.Endpoints.Auth
{
    [Route(AuthRoutes.Register)]
    public class Register : BaseAsyncEndpoint
        .WithRequest<RegisterUserRequest>
        .WithoutResponse
    {
        private readonly IMediator _mediator;

        public Register(IMediator mediator) => _mediator = mediator;
        
        [HttpPost]
        [SwaggerOperation(Description = "Register new user and returns token",
            Summary = "Register new user",
            OperationId = "Auth.Register",
            Tags = new []{ "Auth" })]
        [SwaggerResponse(200,"User registered successfully")]
        [SwaggerResponse(400,"Some error occured during registration")]
        [Produces("application/json")]
        [Consumes("application/json")]
        public override async Task<ActionResult> HandleAsync(
            [SwaggerRequestBody("User register payload",Required = true)]RegisterUserRequest registerUserRequest, 
            CancellationToken cancellationToken = new())
        {
            await _mediator.Send(new RegisterUserCommand(registerUserRequest), cancellationToken);
            return Ok();
        }
    }
}