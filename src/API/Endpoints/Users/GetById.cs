using System.Threading;
using System.Threading.Tasks;
using API.Routes;
using Application.Common.DTOs.Users;
using Application.Common.Models;
using Application.Queries.Users;
using Ardalis.ApiEndpoints;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace API.Endpoints.Users
{
    [Route(ApiRoutes.User)]
    public class GetById : BaseAsyncEndpoint
        .WithRequest<string>
        .WithResponse<IResponse<GetUserDto>>
    {
        private readonly IMediator _mediator;

        public GetById(IMediator mediator) => _mediator = mediator;
        
        [HttpGet("GetById")]
        [SwaggerOperation(Description = "Returns User by id",
            Summary = "Returns User by id",
            OperationId = "User.GetById",
            Tags = new []{ "User" })]
        [SwaggerResponse(200,"User Based On Id",typeof(IResponse<GetUserDto>))]
        [SwaggerResponse(400,"No User Can't Be Found With Provided Id",typeof(IResponse<GetUserDto>))]
        [Produces("application/json")]
        [Consumes("application/json")]
        public override async Task<ActionResult<IResponse<GetUserDto>>> HandleAsync([FromQuery,SwaggerParameter("User Id Which Will Be Retrieved",Required = true)]string id, 
            CancellationToken cancellationToken = new())
        {
            var result = await _mediator.Send(new GetUserQuery(x => x.Id == id), cancellationToken);
            return result.Succeeded ? Ok(result) : BadRequest(result);
        }
    }
}