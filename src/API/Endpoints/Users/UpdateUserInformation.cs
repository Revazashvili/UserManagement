using System.Threading;
using System.Threading.Tasks;
using API.Routes;
using Application.Commands.Users;
using Application.Common.DTOs.Users;
using Application.Common.Models;
using Ardalis.ApiEndpoints;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace API.Endpoints.Users;

[Route(UserRoutes.UpdateInfo)]
public class UpdateUserInformation : BaseAsyncEndpoint
    .WithRequest<UpdateUserInformationRequest>
    .WithResponse<bool>
{
    private readonly IMediator _mediator;

    public UpdateUserInformation(IMediator mediator) => _mediator = mediator;
        
    [HttpPut]
    [SwaggerOperation(Description = "Update user additional information",
        Summary = "Update user additional information",
        OperationId = "User.UpdateUserInformation",
        Tags = new []{ "User" })]
    [SwaggerResponse(200,"User information updated successfully",typeof(IResponse<bool>))]
    [SwaggerResponse(400,"Can't be found user with provided id",typeof(IResponse<bool>))]
    [Produces("application/json")]
    [Consumes("application/json")]
    [Authorize(JwtBearerDefaults.AuthenticationScheme)]
    public override async Task<ActionResult<bool>> HandleAsync(
        [FromBody,SwaggerRequestBody("Update user information payload")]UpdateUserInformationRequest updateUserInformationRequest, 
        CancellationToken cancellationToken = new())
    {
        return Ok(await _mediator.Send(new UpdateUserInformationCommand(updateUserInformationRequest),
            cancellationToken));
    }
}