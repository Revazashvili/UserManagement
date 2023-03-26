using Application.Common.Interfaces;
using Application.Common.Settings;
using Domain.Entities;

namespace Infrastructure.Services;

internal sealed class RefreshTokenService : IRefreshTokenService
{
    private readonly ITokenGenerator _tokenGenerator;
    private readonly JwtSettings _jwtSettings;

    public RefreshTokenService(ITokenGenerator tokenGenerator, JwtSettings jwtSettings) =>
        (_tokenGenerator, _jwtSettings) = (tokenGenerator, jwtSettings);

    public string Generate(User user) => _tokenGenerator.Generate(new GenerateTokenRequest(
        _jwtSettings.RefreshTokenSecret,
        _jwtSettings.Issuer, _jwtSettings.Audience,
        _jwtSettings.RefreshTokenExpirationMinutes)).Token;
}