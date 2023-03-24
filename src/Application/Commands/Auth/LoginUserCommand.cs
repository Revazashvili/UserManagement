using System.Threading;
using System.Threading.Tasks;
using Application.Common.DTOs.Auth;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Common.Wrappers;
using Domain.Entities;
using Domain.Exceptions;
using Forbids;
using Microsoft.AspNetCore.Identity;

namespace Application.Commands.Auth;

public record LoginUserCommand(LoginUserRequest LoginUserRequest) : IRequestWrapper<AuthenticateResponse>;
    
internal sealed class LoginUserCommandHandler : IHandlerWrapper<LoginUserCommand,AuthenticateResponse>
{
    private readonly IAuthenticateService _authenticateService;
    private readonly IForbid _forbid;
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;

    public LoginUserCommandHandler(UserManager<User> userManager, SignInManager<User> signInManager, 
        IAuthenticateService authenticateService,IForbid forbid)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _authenticateService = authenticateService;
        _forbid = forbid;
    }

    public async Task<IResponse<AuthenticateResponse>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.LoginUserRequest.Email);
        _forbid.Null(user, UserNotFoundException.Instance);
        var signInResult =
            await _signInManager.PasswordSignInAsync(user, request.LoginUserRequest.Password, false, false);
        _forbid.False(signInResult.Succeeded, SignInException.Instance);
        return Response.Success(await _authenticateService.Authenticate(user, cancellationToken));
    }
}