using System.Threading;
using System.Threading.Tasks;
using Application.Common.DTOs.Auth;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Common.Wrappers;
using Domain.Entities;
using Domain.Exceptions;
using Forbids;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Commands.Auth;

public record RefreshCommand(RefreshRequest RefreshRequest) : IRequestWrapper<AuthenticateResponse>{}
    
public class RefreshCommandHandler : IHandlerWrapper<RefreshCommand,AuthenticateResponse>
{
    private readonly IAuthenticateService _authenticateService;
    private readonly IForbid _forbid;
    private readonly IRefreshTokenValidator _refreshTokenValidator;
    private readonly IApplicationDbContext _context;
    private readonly UserManager<User> _userManager;

    public RefreshCommandHandler(IRefreshTokenValidator refreshTokenValidator, IApplicationDbContext context,
        UserManager<User> userManager,IAuthenticateService authenticateService,IForbid forbid)
    {
        _refreshTokenValidator = refreshTokenValidator;
        _context = context;
        _userManager = userManager;
        _authenticateService = authenticateService;
        _forbid = forbid;
    }

    public async Task<IResponse<AuthenticateResponse>> Handle(RefreshCommand request, CancellationToken cancellationToken)
    {
        var refreshRequest = request.RefreshRequest;
        var isValidRefreshToken = _refreshTokenValidator.Validate(refreshRequest.RefreshToken);
        _forbid.False(isValidRefreshToken, new InvalidRefreshTokenException());
        var refreshToken =
            await _context.RefreshTokens.FirstOrDefaultAsync(x => x.Token == refreshRequest.RefreshToken,
                cancellationToken);
        _forbid.Null(refreshToken, new InvalidRefreshTokenException());
        _context.RefreshTokens.Remove(refreshToken);
        await _context.SaveChangesAsync(cancellationToken);
            
        var user = await _userManager.FindByIdAsync(refreshToken.UserId);
        _forbid.Null(user, new UserNotFoundException());
        return Response.Success(await _authenticateService.Authenticate(user, cancellationToken));
    }
}