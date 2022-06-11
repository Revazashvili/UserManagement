using System.Threading;
using System.Threading.Tasks;
using Application.Common.DTOs.Auth;
using Domain.Entities;
using Netjection;

namespace Application.Common.Interfaces;

/// <summary>
/// Interface for authentication.
/// </summary>
[InjectAsScoped]
public interface IAuthenticateService
{
    /// <summary>
    /// Authenticates user.
    /// Takes responsibilities to generate access and refresh token, save refresh token in database
    /// and return instance of <see cref="AuthenticateResponse"/> class. 
    /// </summary>
    /// <param name="user">The user.</param>
    /// <param name="cancellationToken">Instance of <see cref="CancellationToken"/>.</param>
    Task<AuthenticateResponse> Authenticate(User user,CancellationToken cancellationToken);
}