using Application.Common.Users.SwaggerSchemaFilters;
using Swashbuckle.AspNetCore.Annotations;

namespace Application.Common.DTOs.Users
{
    [SwaggerSchemaFilter(typeof(LoginUserDtoSchemaFilter))]
    [SwaggerSchema(Required = new[] { "User" })]
    public class LoginUserDto
    {
        [SwaggerSchema(Required = new[] { "The User Email" })]
        public string Email { get; set; }
        [SwaggerSchema(Required = new[] { "The User Password" })]
        public string Password { get; set; }
    }
}