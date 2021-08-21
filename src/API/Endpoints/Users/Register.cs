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
    public class Register : BaseAsyncEndpoint
        .WithRequest<RegisterUserDto>
        .WithResponse<IResponse<string>>
    {
        private readonly IMediator _mediator;

        public Register(IMediator mediator) => _mediator = mediator;
        
        [HttpPost("Register")]
        [SwaggerOperation(Description = "Register New User And Sends Confirmation Email",
            Summary = "Register New User",
            OperationId = "User.Register",
            Tags = new []{ "User" })]
        [SwaggerResponse(200,"User Registered successfully",typeof(IResponse<string>))]
        [SwaggerResponse(400,"Some Error Occured During Registration",typeof(IResponse<string>))]
        [Produces("application/json")]
        [Consumes("application/json")]
        public override async Task<ActionResult<IResponse<string>>> HandleAsync([SwaggerRequestBody("User Register Payload",Required = true)]RegisterUserDto request, 
            CancellationToken cancellationToken = new())
        {
            var result = await _mediator.Send(new RegisterUserCommand(request), cancellationToken);
            return result.Succeeded ? Ok(result) : BadRequest(result);
        }
    }
}