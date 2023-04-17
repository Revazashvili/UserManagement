using Microsoft.AspNetCore.Mvc.Testing;

namespace API.IntegrationTests.Common;

public class TestingApiFactory<TEntryPoint> 
    : WebApplicationFactory<TEntryPoint> 
    where TEntryPoint : Program
{
}