using System.Net.Http.Json;
using API.IntegrationTests.Common;

namespace API.IntegrationTests.Extensions;

internal static class HttpClientExtensions
{
    /// <summary>
    /// Sends a POST request to the specified Uri containing <see cref="request"/> serialized as JSON in request body and returns <see cref="Response{T}"/>.
    /// </summary>
    /// <param name="httpClient">The client used to send the request.</param>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <param name="request">The request to serialize.</param>
    /// <typeparam name="TRequest">The type of the request to serialize.</typeparam>
    /// <typeparam name="TResponse">The type of the response to deserialize as generic in <see cref="Response{T}"/>.</typeparam>
    /// <returns></returns>
    internal static async Task<Response<TResponse>> PostAsync<TRequest,TResponse>(
        this HttpClient httpClient, string? requestUri, TRequest request)
    {
        var responseMessage = await httpClient.PostAsJsonAsync(requestUri,request);
        var response = await responseMessage.Content.ReadFromJsonAsync<Common.Response<TResponse>>();
        return response!;
    }
}