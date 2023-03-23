using Netjection;

namespace Application.Common.Interfaces;

/// <summary>
/// Interface for generating refresh token.
/// </summary>
[InjectAsScoped]
public interface IRefreshTokenService : ITokenService { }