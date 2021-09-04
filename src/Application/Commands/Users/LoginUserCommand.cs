using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.DTOs.Users;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Common.Wrappers;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Application.Commands.Users
{
    public record LoginUserCommand(LoginUserDto LoginUserDto) : IRequestWrapper<string>{}
    
    public class LoginUserCommandHandler : IHandlerWrapper<LoginUserCommand,string>
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ITokenService _tokenService;

        public LoginUserCommandHandler(UserManager<User> userManager, SignInManager<User> signInManager,
            ITokenService tokenService) =>
            (_userManager, _signInManager, _tokenService) = (userManager, signInManager, tokenService);

        public async Task<IResponse<string>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.LoginUserDto.Email);
            if (user is null) throw new Exception("Can't find user with provided email");
            var signInResult =
                await _signInManager.PasswordSignInAsync(user, request.LoginUserDto.Password, false, false);
            if (!signInResult.Succeeded) throw new Exception("error occured while signing in user");
            return Response.Success(_tokenService.Generate(user));
        }
    }
}