using System.Threading;
using System.Threading.Tasks;
using Application.Common.Models;
using Application.Common.Wrappers;
using Domain.Entities;
using Domain.Exceptions;
using Forbids;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Commands.Users;

public record DeleteUserCommand(string Id) : IRequestWrapper<bool>;
    
internal sealed class DeleteUserCommandHandler : IHandlerWrapper<DeleteUserCommand,bool>
{
    private readonly UserManager<User> _userManager;
    private readonly IForbid _forbid;

    public DeleteUserCommandHandler(UserManager<User> userManager, IForbid forbid) =>
        (_userManager, _forbid) = (userManager, forbid);

    public async Task<IResponse<bool>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        _forbid.Null(user, UserNotFoundException.Instance);
        var deleteResult = await _userManager.DeleteAsync(user);
        return new Response<bool>(deleteResult.Succeeded, deleteResult.Succeeded);
    }
}