using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Application.Common.Users.SwaggerSchemaFilters
{
    public class RegisterUserDtoSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            schema.Example = new OpenApiObject
            {
                ["Email"] = new OpenApiString("test@test.ge"),
                ["Password"] = new OpenApiString("yourstringpassword"),
                ["ConfirmPassword"] = new OpenApiString("yourstringpassword")
            };
        }
    }
}