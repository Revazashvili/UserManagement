using Swashbuckle.AspNetCore.Annotations;

namespace Application.Common.DTOs.Users
{
    [SwaggerSchema(ReadOnly = true)]
    public class VerifyEmailDto
    {
        [SwaggerSchema(ReadOnly = true,Required = new []{"The User Identifier"})]
        public string UserId { get; set; }
        [SwaggerSchema(ReadOnly = true,Required = new []{"Token For Email Confirmation"})]
        public string Token { get; set; }
    }
}