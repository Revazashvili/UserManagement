using Domain.Entities;

namespace Application.Common.Interfaces;

/// <summary>
/// Interface for generating token.
/// </summary>
public interface ITokenService
{
    /// <summary>
    /// Generates token based on user information.
    /// </summary>
    /// <param name="user"><see cref="User"/> instance.</param>
    /// <returns>Generated token.</returns>
    string Generate(User user);
}