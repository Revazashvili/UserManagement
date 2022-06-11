using Netjection;

namespace Application.Common.Interfaces;

/// <summary>
/// Interface for validating refresh token.
/// </summary>
[InjectAsScoped]
public interface IRefreshTokenValidator
{
    /// <summary>
    /// Validates refresh token.
    /// </summary>
    /// <param name="refreshToken">The refresh token.</param>
    /// <returns>True if token is valid,otherwise false.</returns>
    bool Validate(string refreshToken);
}