using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Infra;

partial class DataverseHttpApi
{
    public async ValueTask<Result<TOut?, Failure<DataverseFailureCode>>> InvokeAsync<TIn, TOut>(
        DataverseHttpRequest<TIn> request, CancellationToken cacncellationToken)
        where TIn : notnull
    {
        using var httpClient = CreateHttpClient(request.Url);
        using var httpRequest = BuildRequestMessage(request);

        var httpResponse = await httpClient.SendAsync(httpRequest, cacncellationToken).ConfigureAwait(false);
        return await ReadResultAsync<TOut>(httpResponse, cacncellationToken).ConfigureAwait(false);
    }

    private static HttpRequestMessage BuildRequestMessage<TIn>(DataverseHttpRequest<TIn> request)
        where TIn : notnull
    {
        var httpRequest = new HttpRequestMessage
        {
            Method = request.Verb switch
            {
                DataverseHttpVerb.Get       => HttpMethod.Get,
                DataverseHttpVerb.Post      => HttpMethod.Post,
                DataverseHttpVerb.Patch     => HttpMethod.Patch,
                DataverseHttpVerb.Delete    => HttpMethod.Delete,
                _                           => HttpMethod.Post
            },
            Content = request.Content.Map(BuildRequestJsonBody).OrDefault()
        };

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

    private static async ValueTask<Result<TOut?, Failure<DataverseFailureCode>>> ReadResultAsync<TOut>(
        HttpResponseMessage response, CancellationToken cancellationToken)
    {
        if (response.IsSuccessStatusCode && typeof(TOut) == typeof(Unit))
        {
            return default(TOut);
        }

        var body = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
        if (response.IsSuccessStatusCode)
        {
            return DeserializeSuccess<TOut>(body);
        }

        var failureJson = DeserializeFailure(body: body, mediaType: response.Content.Headers.ContentType?.MediaType);
        return ToDataverseFailure(failureJson, body, response.StatusCode);
    }
}