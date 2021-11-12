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

namespace API.Endpoints.Auth;

[Route(AuthRoutes.Refresh)]
public class Refresh : BaseAsyncEndpoint
    .WithRequest<RefreshRequest>
    .WithResponse<IResponse<AuthenticateResponse>>
{
    private readonly IMediator _mediator;

    public Refresh(IMediator mediator) => _mediator = mediator;
        
    [HttpPost]
    [SwaggerOperation(Description = "Refreshes token",
        Summary = "Refresh token",
        OperationId = "Auth.Refresh",
        Tags = new []{ "Auth" })]
    [SwaggerResponse(200,"token refreshed in successfully")]
    [Produces("application/json")]
    [Consumes("application/json")]
    public override async Task<ActionResult<IResponse<AuthenticateResponse>>> HandleAsync(
        [FromBody,SwaggerRequestBody("Refresh token payload")]RefreshRequest refreshRequest, 
        CancellationToken cancellationToken = new())
    {
        return Ok(await _mediator.Send(new RefreshCommand(refreshRequest), cancellationToken));
    }
}