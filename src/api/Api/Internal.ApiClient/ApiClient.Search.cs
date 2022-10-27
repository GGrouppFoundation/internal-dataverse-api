using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace GGroupp.Infra;

partial class DataverseApiClient
{
    public ValueTask<Result<DataverseSearchOut, Failure<DataverseFailureCode>>> SearchAsync(
        DataverseSearchIn input, CancellationToken cancellationToken = default)
    {
        _ = input ?? throw new ArgumentNullException(nameof(input));

        if (cancellationToken.IsCancellationRequested)
        {
            return GetCanceledAsync<DataverseSearchOut>(cancellationToken);
        }

        return InnerSearchAsync(input, cancellationToken);
    }
    
    private async ValueTask<Result<DataverseSearchOut, Failure<DataverseFailureCode>>> InnerSearchAsync(
        DataverseSearchIn input, CancellationToken cancellationToken)
    {
        using var httpClient = CreateSearchHttpClient();

        var searchIn = new DataverseSearchJsonIn(input.Search)
        {
            Entities = input.Entities,
            Facets = input.Facets,
            Filter = input.Filter,
            ReturnTotalRecordCount = input.ReturnTotalRecordCount,
            Skip = input.Skip,
            Top = input.Top,
            OrderBy = input.OrderBy,
            SearchMode = input.SearchMode switch
            {
                null => null,
                DataverseSearchMode.Any => DataverseSearchModeJson.Any,
                _ => DataverseSearchModeJson.All
            },
            SearchType = input.SearchType switch
            {
                null => null,
                DataverseSearchType.Simple => DataverseSearchTypeJson.Simple,
                _ => DataverseSearchTypeJson.Full
            }
        };

        var requestMessage = new HttpRequestMessage()
        { 
            Method = HttpMethod.Post,
            Content = DataverseHttpHelper.BuildRequestJsonBody(searchIn) 
        };

        var response = await httpClient.SendAsync(requestMessage, cancellationToken).ConfigureAwait(false);
        var result = await response.ReadDataverseResultAsync<DataverseSearchJsonOut>(cancellationToken).ConfigureAwait(false);

        return result.MapSuccess(MapSuccess);

        static DataverseSearchOut MapSuccess(DataverseSearchJsonOut? @out)
            =>
            new(
                @out?.TotalRecordCount ?? default,
                @out?.Value?.Select(MapJsonItem).ToArray());

        static DataverseSearchItem MapJsonItem(DataverseSearchJsonItem jsonItem)
            =>
            new(
                searchScore: jsonItem.SearchScore,
                entityName: jsonItem.EntityName,
                objectId: jsonItem.ObjectId,
                extensionData: jsonItem.ExtensionData?.Select(MapJsonFieldValue).ToArray());

        static KeyValuePair<string, DataverseSearchJsonValue> MapJsonFieldValue(KeyValuePair<string, JsonElement> source)
            =>
            new(
                key: source.Key,
                value: new(source.Value));
    }
}
