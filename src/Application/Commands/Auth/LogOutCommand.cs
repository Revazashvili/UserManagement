using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Common.Wrappers;
using Domain.Entities;
using Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Commands.Auth
{
    public record LogOutCommand : IRequestWrapper<Unit>{} 
    
    public class LogOutCommandHandler : IHandlerWrapper<LogOutCommand,Unit>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly SignInManager<User> _signInManager;
        private readonly IApplicationDbContext _context;

        public LogOutCommandHandler(IHttpContextAccessor httpContextAccessor,SignInManager<User> signInManager,IApplicationDbContext context)
        {
            _httpContextAccessor = httpContextAccessor;
            _signInManager = signInManager;
            _context = context;
        }

        public async Task<IResponse<Unit>> Handle(LogOutCommand request, CancellationToken cancellationToken)
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue("id");
            if (string.IsNullOrEmpty(userId)) throw new UserNotFoundException();
            await _signInManager.SignOutAsync();
            var refreshTokens = await _context.RefreshTokens
                .Where(x => x.UserId == userId)
                .ToListAsync(cancellationToken);
            _context.RefreshTokens.RemoveRange(refreshTokens);
            await _context.SaveChangesAsync(cancellationToken);
            return Response.Success(Unit.Value);
        }
    }
}