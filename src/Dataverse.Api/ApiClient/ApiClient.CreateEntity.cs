#nullable enable

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace GGroupp.Infra
{
    partial class DataverseApiClient
    {
        public ValueTask<Result<DataverseEntityCreateOut<TResponseJson>, Failure<int>>> CreateEntityAsync<TRequestJson, TResponseJson>(
            DataverseEntityCreateIn<TRequestJson> input, CancellationToken cancellationToken = default)
        {
            _ = input ?? throw new ArgumentNullException(nameof(input));
            if (cancellationToken.IsCancellationRequested)
            {
                return ValueTask.FromCanceled<Result<DataverseEntityCreateOut<TResponseJson>, Failure<int>>>(cancellationToken);
            }

            return InternalCreateEntityAsync<TRequestJson, TResponseJson>(input, cancellationToken);
        }

        private async ValueTask<Result<DataverseEntityCreateOut<TResponseJson>, Failure<int>>> InternalCreateEntityAsync<TRequestJson, TResponseJson>(
            DataverseEntityCreateIn<TRequestJson> input, CancellationToken cancellationToken = default)
        {
            var httpClient = await DataverseHttpHelper.CreateHttpClientAsync(messageHandler, clientConfiguration);

            var entitiyCreateUrl = BuildEntityCreateUrl(input);

            using var content = BuildEntityCreateContent(input);

            var response = await httpClient.PostAsync(entitiyCreateUrl, content, cancellationToken).ConfigureAwait(false);
            var result = await response.ReadDataverseResultAsync<TResponseJson>(cancellationToken).ConfigureAwait(false);

            return result.MapSuccess(e => new DataverseEntityCreateOut<TResponseJson>(e));
        }

        private static string BuildEntityCreateUrl<TRequestJson>(DataverseEntityCreateIn<TRequestJson> input)
            =>
            Pipeline.Pipe<IReadOnlyCollection<KeyValuePair<string, string>>>(
                new Dictionary<string, string>
                {
                    ["$select"] = QueryParametersBuilder.BuildOdataParameterValue(input.SelectFields)
                })
            .Pipe(
                QueryParametersBuilder.BuildQueryString)
            .Pipe(
                queryString => $"{input.EntityPluralName}{queryString}");

        private static HttpContent BuildEntityCreateContent<TRequestJson>(DataverseEntityCreateIn<TRequestJson> input)
            =>
            Pipeline.Pipe(
                new StringContent(
                    JsonSerializer.Serialize(input.EntityData),
                    Encoding.UTF8,
                    MediaTypeNames.Application.Json))
            .Pipe(contetnt =>
                {
                    contetnt.Headers.Add("Prefer", "return=representation");
                    return contetnt;
                });
    }
}