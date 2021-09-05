using System;
using System.Linq;
using System.Linq.Expressions;
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
    public record GetUserQuery(Expression<Func<User,bool>> Predicate) : IRequestWrapper<GetUserRequest>{}
    
    public class GetUserQueryHandler : IHandlerWrapper<GetUserQuery,GetUserRequest>
    {
        private readonly UserManager<User> _userManager;

        public GetUserQueryHandler(UserManager<User> userManager) => _userManager = userManager;

        public async Task<IResponse<GetUserRequest>> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(request.Predicate, cancellationToken);
            return Response.Success(user.Adapt<GetUserRequest>());
        }
    }
}