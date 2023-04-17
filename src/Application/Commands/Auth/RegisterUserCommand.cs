using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.DTOs.Auth;
using Application.Common.Models;
using Application.Common.Wrappers;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Commands.Auth;

public record RegisterUserCommand(RegisterUserRequest RegisterUserRequest) : IRequestWrapper<Unit>;

internal sealed class RegisterUserCommandHandler : IHandlerWrapper<RegisterUserCommand,Unit>
{
    private readonly UserManager<User> _userManager;
    public RegisterUserCommandHandler(UserManager<User> userManager) => _userManager = userManager;
    
    public async Task<IResponse<Unit>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var user = new User(request.RegisterUserRequest.Email);
        var createResult = await _userManager.CreateAsync(user, request.RegisterUserRequest.Password);
        return createResult.Succeeded
            ? Response.Success(Unit.Value)
            : Response.Fail<Unit>(createResult.Errors.Select(error => error.Description).ToList());
    }
}