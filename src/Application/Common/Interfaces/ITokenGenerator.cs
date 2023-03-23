using System.Collections.Generic;
using System.Security.Claims;
using Netjection;

namespace Application.Common.Interfaces;

/// <summary>
/// A Request that contains information needed to generate a security token.
/// </summary>
/// <param name="SecretKey">The secret key to be used to encrypt the security token.</param>
/// <param name="Issuer">The issuer of the security token.</param>
/// <param name="Audience">The intended audience of the security token.</param>
/// <param name="Expires">The duration in minutes for which the security token will be valid.</param>
/// <param name="Claims">Optional claims to be included in the security token.</param>
public record GenerateTokenRequest(string SecretKey, string Issuer, string Audience, double Expires, IEnumerable<Claim>? Claims = null);

/// <summary>
/// A Response that contains the generated security token.
/// </summary>
/// <param name="Token">The generated security token as a string.</param>
public record GenerateTokenResponse(string Token);

/// <summary>
/// Interface for a token generator that generates security tokens.
/// </summary>
[InjectAsScoped]
public interface ITokenGenerator
{
    /// <summary>
    /// Generates a security token based on the provided request.
    /// </summary>
    /// <param name="generateTokenRequest">The request containing information about the token to be generated.</param>
    /// <returns>The generated security token response.</returns>
    GenerateTokenResponse Generate(GenerateTokenRequest generateTokenRequest);
}