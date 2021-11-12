using System.Threading;
using System.Threading.Tasks;
using API.Routes;
using Application.Commands.Auth;
using Ardalis.ApiEndpoints;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace API.Endpoints.Auth;

[Route(AuthRoutes.LogOut)]
public class LogOut : BaseAsyncEndpoint
    .WithoutRequest
    .WithoutResponse
{
    private readonly IMediator _mediator;

    public LogOut(IMediator mediator) => _mediator = mediator;

    [HttpPost, SwaggerOperation(Description = "Logs out user",
         Summary = "LogOut",
         OperationId = "Auth.LogOut",
         Tags = new[] { "Auth" }), 
     SwaggerResponse(204, "User Logged out successfully"),
     SwaggerResponse(400, "User with provided id can not be found"), 
     Produces("application/json"),
     Consumes("application/json"), 
     Authorize(JwtBearerDefaults.AuthenticationScheme)]
    public override async Task<ActionResult> HandleAsync(CancellationToken cancellationToken = new())
    {
        await _mediator.Send(new LogOutCommand(), cancellationToken);
        return NoContent();
    }
}