namespace API.IntegrationTests.Common;

public class BaseClassFixture : IClassFixture<TestingApiFactory<Program>>
{
    protected readonly HttpClient Client;

    public BaseClassFixture(TestingApiFactory<Program> testingApiFactory) =>
        Client = testingApiFactory.CreateClient();
}