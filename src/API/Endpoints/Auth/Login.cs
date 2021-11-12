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

[Route(AuthRoutes.Login)]
public class Login : BaseAsyncEndpoint
    .WithRequest<LoginUserRequest>
    .WithResponse<IResponse<string>>
{
    private readonly IMediator _mediator;

    public Login(IMediator mediator) => _mediator = mediator;

    [HttpPost,
     SwaggerOperation(Description = "Signs in provided user and return token",
         Summary = "Sign In",
         OperationId = "Auth.Login",
         Tags = new[] { "Auth" }),
     SwaggerResponse(200, "User logged in successfully", typeof(IResponse<string>)),
     SwaggerResponse(400, "User with provided email can not be found", typeof(IResponse<string>)),
     Produces("application/json"), Consumes("application/json")]
    public override async Task<ActionResult<IResponse<string>>> HandleAsync(
        [SwaggerRequestBody("User login payload", Required = true)]
        LoginUserRequest loginUserRequest,
        CancellationToken cancellationToken = new()) =>
        Ok(await _mediator.Send(new LoginUserCommand(loginUserRequest), cancellationToken));
}