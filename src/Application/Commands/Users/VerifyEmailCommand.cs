using System.Threading;
using System.Threading.Tasks;
using Application.Common.Models;
using Application.Common.Wrappers;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Application.Commands.Users
{
    public record VerifyEmailCommand(string UserId,string Token) : IRequestWrapper<bool>{}
    
    public class VerifyEmailCommandHandler : IHandlerWrapper<VerifyEmailCommand,bool>
    {
        private readonly UserManager<User> _userManager;

        public VerifyEmailCommandHandler(UserManager<User> userManager) => _userManager = userManager;

        public async Task<IResponse<bool>> Handle(VerifyEmailCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user is null) return Response.Fail<bool>("Can't find user with provided id");
            var emailConfirmResult = await _userManager.ConfirmEmailAsync(user, request.Token);
            return new Response<bool>(emailConfirmResult.Succeeded, emailConfirmResult.Succeeded);
        }
    }
}