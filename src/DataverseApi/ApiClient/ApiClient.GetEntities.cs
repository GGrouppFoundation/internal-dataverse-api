#nullable enable

using System;
using System.Collections.Generic;
using System.Net.Mime;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace GGroupp
{
    internal sealed partial class DataverseApiClient
    {
        public ValueTask<Result<DataverseEntitiesGetOut<TEntityJson>, Failure<int>>> GetEntitiesAsync<TEntityJson>(
            DataverseEntitiesGetIn input, CancellationToken cancellationToken = default)
        {
            _ = input ?? throw new ArgumentNullException(nameof(input));
            if(cancellationToken.IsCancellationRequested)
            {
                return ValueTask.FromCanceled<Result<DataverseEntitiesGetOut<TEntityJson>, Failure<int>>>(cancellationToken);
            }

            return InternalGetEntitiesAsync<TEntityJson>(input, cancellationToken);
        }

        private async ValueTask<Result<DataverseEntitiesGetOut<TEntityJson>, Failure<int>>> InternalGetEntitiesAsync<TEntityJson>(
            DataverseEntitiesGetIn input, CancellationToken cancellationToken)
        {
            var httpClient = await HttpClientFactory.CreateHttpClientAsync(clientConfiguration, messageHandler); 
            var entitiesGetUrl = BuildEntitiesGetUrl(input);

            var response = await httpClient.GetAsync(entitiesGetUrl, cancellationToken).ConfigureAwait(false);
            var body = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                var output = JsonSerializer.Deserialize<DataverseEntitiesJsonGetOut<TEntityJson>>(body);
                return new DataverseEntitiesGetOut<TEntityJson>(output?.Value);
            }
            if(response.Content.Headers.ContentType?.MediaType != MediaTypeNames.Application.Json)
            {
                return Failure.Create<int>(default, body);
            }

            var failureJson = JsonSerializer.Deserialize<DataverseFailureJson>(body);
            return MapDataverseFailureJson(failureJson);
        }
        
        private static string BuildEntitiesGetUrl(DataverseEntitiesGetIn input)
            =>
            Pipeline.Pipe<IReadOnlyCollection<KeyValuePair<string, string>>>(
                new Dictionary<string, string>
                { 
                    ["$select"] = QueryParametersBuilder.BuildOdataParameterValue(input.SelectFields),
                    ["$filter"] = input.Filter
                })
            .Pipe(
                QueryParametersBuilder.BuildQueryString)
            .Pipe(
                queryString => input.EntitiesName + queryString);
    }
}