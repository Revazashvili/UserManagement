using System.Threading;
using System.Threading.Tasks;
using Application.Common.DTOs.Auth;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Common.Wrappers;
using Domain.Entities;
using Domain.Exceptions;
using Microsoft.AspNetCore.Identity;

namespace Application.Commands.Auth
{
    public record LoginUserCommand(LoginUserRequest LoginUserRequest) : IRequestWrapper<AuthenticateResponse>{}
    
    public class LoginUserCommandHandler : IHandlerWrapper<LoginUserCommand,AuthenticateResponse>
    {
        private readonly IAuthenticateService _authenticateService;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public LoginUserCommandHandler(UserManager<User> userManager, SignInManager<User> signInManager, IAuthenticateService authenticateService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _authenticateService = authenticateService;
        }

        public async Task<IResponse<AuthenticateResponse>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.LoginUserRequest.Email);
            if (user is null) throw new UserNotFoundException();
            var signInResult =
                await _signInManager.PasswordSignInAsync(user, request.LoginUserRequest.Password, false, false);
            if (!signInResult.Succeeded) throw new SignInException();
            return Response.Success(await _authenticateService.Authenticate(user, cancellationToken));
        }
    }
}