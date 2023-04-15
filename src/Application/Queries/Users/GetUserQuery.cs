using System;
using System.Linq.Expressions;
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

public record GetUserQuery(Expression<Func<User,bool>> Predicate) : IRequestWrapper<GetUserRequest>;

internal sealed class GetUserQueryHandler : IHandlerWrapper<GetUserQuery,GetUserRequest>
{
    private readonly UserManager<User> _userManager;
    private readonly IForbid _forbid;

    public GetUserQueryHandler(UserManager<User> userManager, IForbid forbid) =>
        (_userManager, _forbid) = (userManager, forbid);

    public async Task<IResponse<GetUserRequest>> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(request.Predicate, cancellationToken);
        _forbid.Null(user, UserNotFoundException.Instance);
        return Response.Success(user.Adapt<GetUserRequest>());
    }
}