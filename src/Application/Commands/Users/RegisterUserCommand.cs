using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.DTOs.Users;
using Application.Common.Models;
using Application.Common.Wrappers;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Application.Commands.Users
{
    public record RegisterUserCommand(RegisterUserDto RegisterUserDto) : IRequestWrapper<string>{}
    
    public class RegisterUserCommandHandler : IHandlerWrapper<RegisterUserCommand,string>
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public RegisterUserCommandHandler(UserManager<User> userManager, SignInManager<User> signInManager) =>
            (_userManager, _signInManager) = (userManager, signInManager);
        
        public async Task<IResponse<string>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var user = new User
            {
                Email = request.RegisterUserDto.Email,
                UserName = request.RegisterUserDto.Email
            };
            var createResult = await _userManager.CreateAsync(user, request.RegisterUserDto.Password);
            if (createResult.Succeeded)
            {
                var signInResult = await _signInManager.PasswordSignInAsync(user, request.RegisterUserDto.Password,false,false);
                return signInResult.Succeeded
                    ? Response.Success("User Registered And Sign In Successfully")
                    : Response.Fail<string>("User Registered Successfully,But Can't Sign In. Please Try Again");
            }
            return Response.Fail<string>(createResult.Errors.Select(x=>x.Description).ToList());
        }
    }
}