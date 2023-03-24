using System.Threading;
using System.Threading.Tasks;
using Application.Common.DTOs.Auth;
using Application.Common.Models;
using Application.Common.Wrappers;
using Domain.Entities;
using Domain.Exceptions;
using Forbids;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Commands.Auth;

public record RegisterUserCommand(RegisterUserRequest RegisterUserRequest) : IRequestWrapper<Unit>;

public sealed class RegisterUserCommandHandler : IHandlerWrapper<RegisterUserCommand,Unit>
{
    private readonly UserManager<User> _userManager;
    private readonly IForbid _forbid;

    public RegisterUserCommandHandler(UserManager<User> userManager, IForbid forbid) =>
        (_userManager, _forbid) = (userManager, forbid);
        
    public async Task<IResponse<Unit>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var user = new User(request.RegisterUserRequest.Email);
        var createResult = await _userManager.CreateAsync(user, request.RegisterUserRequest.Password);
        _forbid.False(createResult.Succeeded, RegisterException.Instance);
        return Response.Success(Unit.Value);
    }
}