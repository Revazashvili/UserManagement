using Swashbuckle.AspNetCore.Annotations;

namespace Application.Common.DTOs.Auth;

public class AuthenticateResponse
{
    [SwaggerSchema(Required = new[] { "The access token" })]
    public string AccessToken { get; set; }
    [SwaggerSchema(Required = new[] { "The refresh token" })]
    public string RefreshToken { get; set; }
}