using System;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Infra;

partial class DataverseHttpApi
{
    public async ValueTask<Result<DataverseJsonResponse, Failure<DataverseFailureCode>>> SendJsonAsync(
        DataverseJsonRequest request, CancellationToken cacncellationToken)
    {
        var httpClient = CreateHttpClient();
        using var httpRequest = CreateHttpRequestMessage(request);

        var httpResponse = await httpClient.SendAsync(httpRequest, cacncellationToken).ConfigureAwait(false);
        return await ReadJsonResponseAsync(httpResponse, cacncellationToken).ConfigureAwait(false);
    }

    private HttpRequestMessage CreateHttpRequestMessage(DataverseJsonRequest request)
    {
        var httpRequest = new HttpRequestMessage
        {
            Method = request.Verb switch
            {
                DataverseHttpVerb.Get => HttpMethod.Get,
                DataverseHttpVerb.Post => HttpMethod.Post,
                DataverseHttpVerb.Patch => HttpMethod.Patch,
                DataverseHttpVerb.Delete => HttpMethod.Delete,
                _ => HttpMethod.Post
            },
            RequestUri = new(dataverseBaseUri, request.Url),
            Version = new(HttpVersion)
        };

        if (string.IsNullOrEmpty(request.Content?.Json) is false)
        {
            httpRequest.Content = new StringContent(request.Content.Json, Encoding.UTF8, MediaTypeNames.Application.Json);
        }

        if (request.Headers.IsEmpty)
        {
            return httpRequest;
        }

        foreach (var header in request.Headers)
        {
            httpRequest.Headers.TryAddWithoutValidation(header.Name.Trim(), header.Value);
        }

        return httpRequest;
    }

    private static async ValueTask<Result<DataverseJsonResponse, Failure<DataverseFailureCode>>> ReadJsonResponseAsync(
        HttpResponseMessage response, CancellationToken cancellationToken)
    {
        if (response.IsSuccessStatusCode && response.StatusCode is HttpStatusCode.NoContent)
        {
            return Result.Success<DataverseJsonResponse>(default);
        }

        if (response.IsSuccessStatusCode)
        {
            var body = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
            var content = GetContent(body);

            return new DataverseJsonResponse(content);
        }

        return await ReadJsonFailureAsync(response, cancellationToken).ConfigureAwait(false);
    }

    private static async ValueTask<Failure<DataverseFailureCode>> ReadJsonFailureAsync(
        HttpResponseMessage response, CancellationToken cancellationToken)
    {
        var body = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
        var failureJson = DeserializeFailure(body: body, mediaType: response.Content.Headers.ContentType?.MediaType);

        return ToDataverseFailure(failureJson, body, response.StatusCode);
    }

    private static DataverseJsonContentOut? GetContent(string? body)
    {
        if (string.IsNullOrEmpty(body))
        {
            return null;
        }

        return new(body);
    }
}