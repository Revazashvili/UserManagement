using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.DTOs.Users;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Common.Wrappers;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using NETCore.MailKit.Core;

namespace Application.Commands.Users
{
    public record RegisterUserCommand(RegisterUserDto RegisterUserDto) : IRequestWrapper<string>{}
    
    public class RegisterUserCommandHandler : IHandlerWrapper<RegisterUserCommand,string>
    {
        private readonly UserManager<User> _userManager;
        private readonly IEmailService _emailService;
        private readonly IUrlService _urlService;

        public RegisterUserCommandHandler(UserManager<User> userManager, IEmailService emailService,
            IUrlService urlService) =>
            (_userManager, _emailService, _urlService) = (userManager, emailService, urlService);
        
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
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var link = _urlService.GenerateEmailConfirmationLink(user.Id, token);
                await _emailService.SendAsync ("test@test.com", "Email verify", $"<a href=\"{link}\">Email verify</a>",true);
                return Response.Success("Email has been sent. Please verify it.");
            }
            
            return Response.Fail<string>(createResult.Errors.Select(x=>x.Description).ToList());
        }
    }
}