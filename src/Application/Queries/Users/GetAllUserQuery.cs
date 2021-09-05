using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.DTOs.Users;
using Application.Common.Models;
using Application.Common.Wrappers;
using Domain.Entities;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Queries.Users
{
    public record GetAllUserQuery : IRequestWrapper<IReadOnlyList<GetUserRequest>>{}

    public class GetAllUserQueryHandler : IHandlerWrapper<GetAllUserQuery,IReadOnlyList<GetUserRequest>>
    {
        private readonly UserManager<User> _userManager;
        public GetAllUserQueryHandler(UserManager<User> userManager) => _userManager = userManager;

        public async Task<IResponse<IReadOnlyList<GetUserRequest>>> Handle(GetAllUserQuery request, CancellationToken cancellationToken)
        {
            var users = await _userManager.Users
                .ProjectToType<GetUserRequest>()
                .ToListAsync(cancellationToken);
            return Response.Success<IReadOnlyList<GetUserRequest>>(users);
        }
    }
}