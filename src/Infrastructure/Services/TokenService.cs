using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.Common.Interfaces;
using Application.Common.Settings;
using Domain.Entities;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Services
{
    /// <inheritdoc cref="ITokenService"/>
    public class TokenService : ITokenService
    {
        private readonly JwtSettings _jwtSettings;

        public TokenService(JwtSettings jwtSettings) => _jwtSettings = jwtSettings;

        public string Generate(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.AccessTokenSecret));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            List<Claim> claims = new()
            {
                new Claim("id", user.Id),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.UserName),
            };
            JwtSecurityToken securityToken = new(_jwtSettings.Issuer, _jwtSettings.Audience,
                claims,
                DateTime.UtcNow,
                DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpirationMinutes),
                credentials);
            return new JwtSecurityTokenHandler().WriteToken(securityToken);
        }
    }
}