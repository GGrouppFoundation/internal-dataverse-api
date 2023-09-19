using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Infra;

partial class DataverseHttpApi
{
    public async ValueTask<Result<DataverseChangeSetResponse, Failure<DataverseFailureCode>>> SendChangeSetAsync(
        DataverseChangeSetRequest request, CancellationToken cancellationToken)
    {
        using var httpClient = CreateHttpClient();
        using var httpRequest = CreateHttpRequestMessage(request);

        var httpResponse = await httpClient.SendAsync(httpRequest, cancellationToken).ConfigureAwait(false);
        return await ReadChangeSetResponseAsync(httpResponse, cancellationToken).ConfigureAwait(false);
    }

    private HttpRequestMessage CreateHttpRequestMessage(DataverseChangeSetRequest request)
    {
        var httpRequest = new HttpRequestMessage(HttpMethod.Post, request.Url)
        {
            Version = new(HttpVersion)
        };

        var changeSetContent = new MultipartContent("mixed", $"changeset_{request.ChangeSetId}");

        for (var i = 0; i < request.Requests.Length; i++)
        {
            var jsonContent = new ChangeSetJsonRequestContent(
                request: request.Requests[i],
                contentId: i + 1,
                httpVersion: HttpVersion);

            changeSetContent.Add(jsonContent);
        }

        httpRequest.Content = new MultipartContent("mixed", $"batch_{request.BatchId}")
        {
            changeSetContent
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

    private static async ValueTask<Result<DataverseChangeSetResponse, Failure<DataverseFailureCode>>> ReadChangeSetResponseAsync(
        HttpResponseMessage httpResponse, CancellationToken cancellationToken)
    {
        if (httpResponse.IsSuccessStatusCode is false && httpResponse.Content.IsMimeMultipartContent() is false)
        {
            return await ReadJsonFailureAsync(httpResponse, cancellationToken).ConfigureAwait(false);
        }

        var result = await ParseMultipartContentAsync(httpResponse.Content, cancellationToken).ConfigureAwait(false);
        return result.Forward(CreateChangeSetResponseOrFailure);

        Result<DataverseChangeSetResponse, Failure<DataverseFailureCode>> CreateChangeSetResponseOrFailure(
            FlatArray<DataverseJsonResponse> responses)
            =>
            httpResponse.IsSuccessStatusCode switch
            {
                true => Result.Success<DataverseChangeSetResponse>(new(responses)),
                _ => Failure.Create(DataverseFailureCode.Unknown, GetStatusCodeFailureMessage(httpResponse.StatusCode))
            };
    }

    private static async Task<Result<FlatArray<DataverseJsonResponse>, Failure<DataverseFailureCode>>> ParseMultipartContentAsync(
        HttpContent content, CancellationToken cancellationToken)
    {
        var batchResponseContent = await content.ReadAsMultipartAsync(cancellationToken).ConfigureAwait(false);
        if (batchResponseContent?.Contents?.Count is not > 0)
        {
            return Result.Success<FlatArray<DataverseJsonResponse>>(default);
        }

        var responses = new List<DataverseJsonResponse>();
        foreach (var httpContent in batchResponseContent.Contents)
        {
            //This is true for changesets
            if (httpContent.IsMimeMultipartContent())
            {
                //Recursive call
                var result = await ParseMultipartContentAsync(httpContent, cancellationToken).ConfigureAwait(false);
                if (result.IsFailure)
                {
                    return result.FailureOrThrow();
                }

                responses.AddRange(result.SuccessOrThrow().AsEnumerable());
                continue;
            }

            //This is for individual responses outside of a change set.
            //Must change Content-Type for ReadAsHttpResponseMessageAsync method to work.
            httpContent.Headers.Remove("Content-Type");
            httpContent.Headers.Add("Content-Type", "application/http;msgtype=response");

            var httpResponseMessage = await httpContent.ReadAsHttpResponseMessageAsync(cancellationToken).ConfigureAwait(false);
            if (httpResponseMessage is null)
            {
                continue;
            }

            var jsonResult = await ReadJsonResponseAsync(httpResponseMessage, cancellationToken).ConfigureAwait(false);
            if (jsonResult.IsFailure)
            {
                return jsonResult.FailureOrThrow();
            }

            responses.Add(jsonResult.SuccessOrThrow());
        }

        return responses.ToFlatArray();
    }
}