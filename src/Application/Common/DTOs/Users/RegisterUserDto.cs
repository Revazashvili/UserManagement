using Application.Common.Users.SwaggerSchemaFilters;
using Swashbuckle.AspNetCore.Annotations;

namespace Application.Common.DTOs.Users
{
    [SwaggerSchemaFilter(typeof(RegisterUserDtoSchemaFilter))]
    public class RegisterUserDto
    {
        [SwaggerSchema(Required = new[] { "The User Email" })]
        public string Email { get; set; }
        [SwaggerSchema(Required = new[] { "The User Password" })]
        public string Password { get; set; }
        [SwaggerSchema(Required = new[] { "The User Confirmed Password" })]
        public string ConfirmPassword { get; set; }
    }
}