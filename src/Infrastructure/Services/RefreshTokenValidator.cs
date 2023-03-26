using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Application.Common.Interfaces;
using Application.Common.Settings;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Services;

internal sealed class RefreshTokenValidator : IRefreshTokenValidator
{
    private readonly JwtSettings _jwtSettings;

    public RefreshTokenValidator(JwtSettings jwtSettings) => _jwtSettings = jwtSettings;

    public bool Validate(string refreshToken)
    {
        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.RefreshTokenSecret)),
            ValidIssuer = _jwtSettings.Issuer,
            ValidAudience = _jwtSettings.Audience,
            ClockSkew = TimeSpan.Zero
        };

        JwtSecurityTokenHandler jwtSecurityTokenHandler = new();
        try
        {
            jwtSecurityTokenHandler.ValidateToken(refreshToken, validationParameters, out var _);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}