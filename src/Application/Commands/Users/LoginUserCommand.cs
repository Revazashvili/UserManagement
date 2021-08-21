using System.Threading;
using System.Threading.Tasks;
using Application.Common.DTOs.Users;
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

        public LoginUserCommandHandler(UserManager<User> userManager, SignInManager<User> signInManager) =>
            (_userManager, _signInManager) = (userManager, signInManager);

        public async Task<IResponse<string>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.LoginUserDto.Email);
            if(user is null) return Response.Fail<string>("Can't find user with provided email");
            var signInResult =
                await _signInManager.PasswordSignInAsync(user, request.LoginUserDto.Password, false, false);
            return signInResult.Succeeded
                ? Response.Success("you are signed in")
                : Response.Fail<string>("Error while signing user");
        }
    }
}