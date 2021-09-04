using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Models;
using Application.Common.Wrappers;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Commands.Users
{
    public record DeleteUserCommand(string Id) : IRequestWrapper<bool>{}
    
    public class DeleteUserCommandHandler : IHandlerWrapper<DeleteUserCommand,bool>
    {
        private readonly UserManager<User> _userManager;

        public DeleteUserCommandHandler(UserManager<User> userManager) => _userManager = userManager;

        public async Task<IResponse<bool>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
            if (user is null) throw new Exception("Can't find user with provided id");
            var deleteResult = await _userManager.DeleteAsync(user);
            return new Response<bool>(deleteResult.Succeeded, deleteResult.Succeeded);
        }
    }
}