using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.DTOs.Users;
using Application.Common.Models;
using Application.Common.Wrappers;
using Domain.Entities;
using Domain.Exceptions;
using Forbids;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Queries.Users;

public record GetAllUserQuery : IRequestWrapper<IReadOnlyList<GetUserRequest>>;

internal sealed class GetAllUserQueryHandler : IHandlerWrapper<GetAllUserQuery,IReadOnlyList<GetUserRequest>>
{
    private readonly UserManager<User> _userManager;
    private readonly IForbid _forbid;

    public GetAllUserQueryHandler(UserManager<User> userManager, IForbid forbid) =>
        (_userManager, _forbid) = (userManager, forbid);

    public async Task<IResponse<IReadOnlyList<GetUserRequest>>> Handle(GetAllUserQuery request, CancellationToken cancellationToken)
    {
        var users = await _userManager.Users
            .ProjectToType<GetUserRequest>()
            .ToListAsync(cancellationToken);
        _forbid.NullOrEmpty(users, UserNotFoundException.Instance);
        return Response.Success<IReadOnlyList<GetUserRequest>>(users);
    }
}