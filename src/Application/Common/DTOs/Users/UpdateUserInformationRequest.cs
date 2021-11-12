using Application.Common.SwaggerSchemaFilters.Users;
using Swashbuckle.AspNetCore.Annotations;

namespace Application.Common.DTOs.Users;

[SwaggerSchemaFilter(typeof(UpdateUserInformationDtoSchemaFilter))]
public class UpdateUserInformationRequest
{
    [SwaggerSchema(ReadOnly = true,Required = new []{"The User Email"})]
    public string Email { get; set; }
    [SwaggerSchema(ReadOnly = true,Required = new []{"The User Personal Number"})]
    public string Pin { get; set; }
    [SwaggerSchema(ReadOnly = true,Required = new []{"The Flag Indicating If User is Married"})]
    public bool IsMarried { get; set; }
    [SwaggerSchema(ReadOnly = true,Required = new []{"The Flag Indicating If User is Employed"})]
    public bool Employed { get; set; }
    [SwaggerSchema(ReadOnly = true,Required = new []{"The User Compensation"})]
    public double? Compensation { get; set; }
    [SwaggerSchema(ReadOnly = true,Required = new []{"The User Physical Address"})]
    public string Address { get; set; }
}