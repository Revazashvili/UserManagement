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
    public class VerifyEmail : BaseAsyncEndpoint
        .WithRequest<VerifyEmailDto>
        .WithResponse<IResponse<bool>>
    {
        private readonly IMediator _mediator;

        public VerifyEmail(IMediator mediator) => _mediator = mediator;

        [HttpPost("VerifyEmail")]
        [SwaggerOperation(Description = "Verifies Email With User Id And Token",
            Summary = "Verifies Email",
            OperationId = "User.VerifyEmail",
            Tags = new []{ "User" })]
        [SwaggerResponse(200,"User Email Verified successfully",typeof(IResponse<bool>))]
        [SwaggerResponse(400,"Some Error Occured During Email Verification",typeof(IResponse<bool>))]
        [Produces("application/json")]
        [Consumes("application/json")]
        public override async Task<ActionResult<IResponse<bool>>> HandleAsync([FromRoute,SwaggerRequestBody("Verify Email Payload")]VerifyEmailDto request, CancellationToken cancellationToken = new())
        {
            var result = await _mediator.Send(new VerifyEmailCommand(request.UserId, request.Token),cancellationToken);
            return result.Succeeded ? Ok(result) : BadRequest(result);
        }
    }
}