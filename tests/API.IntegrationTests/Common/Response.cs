namespace API.IntegrationTests.Common;

internal class Response<T>
{
    public T? Data { get; set; } = default;
    public List<string> Errors { get; set; } = new();
    public bool Succeeded { get; set; }
}