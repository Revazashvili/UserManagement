using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Application.Common.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Services;

/// <inheritdoc cref="ITokenGenerator"/>
internal sealed class TokenGenerator : ITokenGenerator
{
    /// <summary>
    /// Generates a security token based on the provided request.
    /// </summary>
    /// <param name="generateTokenRequest">The request containing information about the token to be generated.</param>
    /// <returns>The generated security token response.</returns>
    public GenerateTokenResponse Generate(GenerateTokenRequest generateTokenRequest)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(generateTokenRequest.SecretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        JwtSecurityToken securityToken = new(generateTokenRequest.Issuer,
            generateTokenRequest.Audience,
            generateTokenRequest.Claims,
            DateTime.UtcNow,
            DateTime.UtcNow.AddMinutes(generateTokenRequest.Expires),
            credentials);
        var token = new JwtSecurityTokenHandler().WriteToken(securityToken);
        return new GenerateTokenResponse(token!);
    }
}