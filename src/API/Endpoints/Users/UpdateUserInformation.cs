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
    public class UpdateUserInformation : BaseAsyncEndpoint
        .WithRequest<UpdateUserInformationDto>
        .WithResponse<bool>
    {
        private readonly IMediator _mediator;

        public UpdateUserInformation(IMediator mediator) => _mediator = mediator;
        
        [HttpPut("UpdateUserInformation")]
        [SwaggerOperation(Description = "Update User Additional Information",
            Summary = "Update User Additional Information",
            OperationId = "User.UpdateUserInformation",
            Tags = new []{ "User" })]
        [SwaggerResponse(200,"User Information Updated successfully",typeof(IResponse<bool>))]
        [SwaggerResponse(400,"Can't Be Found User With Provided Id",typeof(IResponse<bool>))]
        [Produces("application/json")]
        [Consumes("application/json")]
        public override async Task<ActionResult<bool>> HandleAsync([SwaggerRequestBody("Update User Information Payload")]UpdateUserInformationDto request, 
            CancellationToken cancellationToken = new())
        {
            var result = await _mediator.Send(new UpdateUserInformationCommand(request), cancellationToken);
            return result.Succeeded ? Ok(result) : BadRequest(result);
        }
    }
}