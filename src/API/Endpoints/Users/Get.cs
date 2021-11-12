using System.Collections.Generic;
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

[Route(UserRoutes.Get)]
public class Get : BaseAsyncEndpoint
    .WithoutRequest
    .WithResponse<IResponse<IReadOnlyList<GetUserRequest>>>
{
    private readonly IMediator _mediator;
    public Get(IMediator mediator) => _mediator = mediator;

    [HttpGet, SwaggerOperation(Description = "Returns all user",
         Summary = "Returns all user",
         OperationId = "User.Get",
         Tags = new[] { "User" }),
     SwaggerResponse(200, "All user from database", typeof(IResponse<IReadOnlyList<GetUserRequest>>)),
     SwaggerResponse(400, "No User can't be found", typeof(IResponse<IReadOnlyList<GetUserRequest>>)),
     Produces("application/json"), Consumes("application/json"),
     Authorize(JwtBearerDefaults.AuthenticationScheme)]
    public override async Task<ActionResult<IResponse<IReadOnlyList<GetUserRequest>>>>
        HandleAsync(CancellationToken cancellationToken = new()) =>
        Ok(await _mediator.Send(new GetAllUserQuery(), cancellationToken));
}