using System.Threading;
using System.Threading.Tasks;
using API.Routes;
using Application.Commands.Users;
using Application.Common.Models;
using Ardalis.ApiEndpoints;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace API.Endpoints.Users
{
    [Route(UserRoutes.Delete)]
    public class Delete : BaseAsyncEndpoint
        .WithRequest<string>
        .WithResponse<IResponse<bool>>
    {
        private readonly IMediator _mediator;

        public Delete(IMediator mediator) => _mediator = mediator;
        
        [HttpDelete]
        [SwaggerOperation(Description = "Deletes user from database with provided id",
            Summary = "Delete user",
            OperationId = "User.Delete",
            Tags = new []{ "User" })]
        [SwaggerResponse(200,"User deleted successfully",typeof(IResponse<bool>))]
        [SwaggerResponse(400,"User can not be found",typeof(IResponse<bool>))]
        [Produces("application/json")]
        [Consumes("application/json")]
        [Authorize(JwtBearerDefaults.AuthenticationScheme)]
        public override async Task<ActionResult<IResponse<bool>>> HandleAsync([FromQuery,SwaggerParameter("User id")]string id,
            CancellationToken cancellationToken = new())
        {
            return Ok(await _mediator.Send(new DeleteUserCommand(id), cancellationToken));
        }
    }
}