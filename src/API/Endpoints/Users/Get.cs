using System.Collections.Generic;
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
    public class Get : BaseAsyncEndpoint
        .WithoutRequest
        .WithResponse<IResponse<IReadOnlyList<GetUserDto>>>
    {
        private readonly IMediator _mediator;
        public Get(IMediator mediator) => _mediator = mediator;
        
        [HttpGet("Users")]
        [SwaggerOperation(Description = "Returns all User",
            Summary = "Returns all User",
            OperationId = "User.GetAll",
            Tags = new []{ "User" })]
        [SwaggerResponse(200,"All User From Database",typeof(IResponse<IReadOnlyList<GetUserDto>>))]
        [SwaggerResponse(400,"No User Can't Be Found",typeof(IResponse<IReadOnlyList<GetUserDto>>))]
        [Produces("application/json")]
        [Consumes("application/json")]
        public override async Task<ActionResult<IResponse<IReadOnlyList<GetUserDto>>>> HandleAsync(CancellationToken cancellationToken = new())
        {
            var result = await _mediator.Send(new GetAllUserQuery(),cancellationToken);
            return result.Succeeded ? Ok(result) : BadRequest(result);
        }
    }
}