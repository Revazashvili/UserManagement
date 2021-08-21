using System.Threading;
using System.Threading.Tasks;
using API.Routes;
using Application.Commands.Users;
using Application.Common.Models;
using Ardalis.ApiEndpoints;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace API.Endpoints.Users
{
    [Route(ApiRoutes.User)]
    public class Delete : BaseAsyncEndpoint
        .WithRequest<string>
        .WithResponse<IResponse<bool>>
    {
        private readonly IMediator _mediator;

        public Delete(IMediator mediator) => _mediator = mediator;
        
        [HttpDelete("Delete")]
        [SwaggerOperation(Description = "Deletes User From Database With Provided Id",
            Summary = "Delete User",
            OperationId = "User.Delete",
            Tags = new []{ "User" })]
        [SwaggerResponse(200,"User deleted successfully",typeof(IResponse<bool>))]
        [SwaggerResponse(400,"User can not be found",typeof(IResponse<bool>))]
        [Produces("application/json")]
        [Consumes("application/json")]
        public override async Task<ActionResult<IResponse<bool>>> HandleAsync([FromQuery,SwaggerParameter("User Id Which Should Be Delete")]string Id,
            CancellationToken cancellationToken = new())
        {
            var result = await _mediator.Send(new DeleteUserCommand(Id), cancellationToken);
            return result.Succeeded ? Ok(result) : BadRequest(result);
        }
    }
}