using System;
using System.Linq;
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
    public record RegisterUserCommand(RegisterUserDto RegisterUserDto) : IRequestWrapper<string>{}
    
    public class RegisterUserCommandHandler : IHandlerWrapper<RegisterUserCommand,string>
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ITokenService _tokenService;

        public RegisterUserCommandHandler(UserManager<User> userManager, SignInManager<User> signInManager,
            ITokenService tokenService) =>
            (_userManager, _signInManager, _tokenService) = (userManager, signInManager, tokenService);
        
        public async Task<IResponse<string>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var user = new User
            {
                Email = request.RegisterUserDto.Email,
                UserName = request.RegisterUserDto.Email
            };
            var createResult = await _userManager.CreateAsync(user, request.RegisterUserDto.Password);
            if (!createResult.Succeeded)
                throw new Exception(createResult.Errors.Select(x => x.Description).FirstOrDefault());
            
            await _signInManager.PasswordSignInAsync(user, request.RegisterUserDto.Password,false,false);
            return Response.Success(_tokenService.Generate(user));
        }
    }
}