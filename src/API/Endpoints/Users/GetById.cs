using System.Threading;
using System.Threading.Tasks;
using API.Routes;
using Application.Common.DTOs.Users;
using Application.Common.Models;
using Application.Queries.Users;
using Ardalis.ApiEndpoints;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace API.Endpoints.Users;

[Route(UserRoutes.GetById)]
public class GetById : BaseAsyncEndpoint
    .WithRequest<string>
    .WithResponse<IResponse<GetUserRequest>>
{
    private readonly IMediator _mediator;

    public GetById(IMediator mediator) => _mediator = mediator;
        
    [HttpGet]
    [SwaggerOperation(Description = "Returns user by id",
        Summary = "Returns user by id",
        OperationId = "User.GetById",
        Tags = new []{ "User" })]
    [SwaggerResponse(200,"User based on id",typeof(IResponse<GetUserRequest>))]
    [SwaggerResponse(400,"No User can't be found with provided id",typeof(IResponse<GetUserRequest>))]
    [Produces("application/json")]
    [Consumes("application/json")]
    [Authorize(JwtBearerDefaults.AuthenticationScheme)]
    public override async Task<ActionResult<IResponse<GetUserRequest>>> HandleAsync(
        [FromQuery,SwaggerParameter("User id",Required = true)]string id, 
        CancellationToken cancellationToken = new())
    {
        return Ok(await _mediator.Send(new GetUserQuery(x => x.Id == id), cancellationToken));
    }
}