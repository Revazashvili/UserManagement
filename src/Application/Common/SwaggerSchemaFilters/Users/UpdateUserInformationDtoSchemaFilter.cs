using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Application.Common.SwaggerSchemaFilters.Users;

public class UpdateUserInformationDtoSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        schema.Example = new OpenApiObject
        {
            ["Email"] = new OpenApiString("test@test.ge"),
            ["Pin"] = new OpenApiString("00116723129"),
            ["IsMarried"] = new OpenApiBoolean(false),
            ["Employed"] = new OpenApiBoolean(true),
            ["Compensation"] = new OpenApiDouble(1200),
            ["Address"] = new OpenApiString("8540 Clay Street North Hills, CA")
        };
    }
}