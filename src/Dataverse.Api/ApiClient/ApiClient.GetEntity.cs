#nullable enable

using System;
using System.Collections.Generic;
using System.Net.Mime;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace GGroupp.Infra
{
    partial class DataverseApiClient
    {
        public ValueTask<Result<DataverseEntityGetOut<TEntityJson>, Failure<int>>> GetEntityAsync<TEntityJson>(
            DataverseEntityGetIn input, CancellationToken cancellationToken = default)
        {
            _ = input ?? throw new ArgumentNullException(nameof(input));
            if(cancellationToken.IsCancellationRequested)
            {
                return ValueTask.FromCanceled<Result<DataverseEntityGetOut<TEntityJson>, Failure<int>>>(cancellationToken);
            }

            return InternalGetEntityAsync<TEntityJson>(input, cancellationToken);
        }

        private async ValueTask<Result<DataverseEntityGetOut<TEntityJson>, Failure<int>>> InternalGetEntityAsync<TEntityJson>(
            DataverseEntityGetIn input, CancellationToken cancellationToken)
        {
            var httpClient = await HttpClientFactory.CreateHttpClientAsync(clientConfiguration, messageHandler); 
            var entitiesGetUrl = BuildEntityGetUrl(input);

            var response = await httpClient.GetAsync(entitiesGetUrl, cancellationToken).ConfigureAwait(false);
            var body = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                DataverseEntityJsonGetOut<TEntityJson>? output = new DataverseEntityJsonGetOut<TEntityJson>()
                {
                    Value = JsonSerializer.Deserialize<TEntityJson>(body)
                };
               
                return new DataverseEntityGetOut<TEntityJson>(output.Value);
            }
            if(response.Content.Headers.ContentType?.MediaType != MediaTypeNames.Application.Json)
            {
                return Failure.Create<int>(default, body);
            }

            var failureJson = JsonSerializer.Deserialize<DataverseFailureJson>(body);
            return MapDataverseFailureJson(failureJson);
        }

        private static string BuildEntityGetUrl(DataverseEntityGetIn input)
            =>
            Pipeline.Pipe<IReadOnlyCollection<KeyValuePair<string, string>>>(
                new Dictionary<string, string>
                {
                    ["$select"] = QueryParametersBuilder.BuildOdataParameterValue(input.SelectFields)
                })
            .Pipe(
                QueryParametersBuilder.BuildQueryString)
            .Pipe(
                queryString => $"{input.EntityPluralName}({input.EntityId}){queryString}");
    }
}