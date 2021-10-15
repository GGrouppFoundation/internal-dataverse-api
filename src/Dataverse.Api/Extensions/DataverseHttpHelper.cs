using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net.Mime;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using static System.FormattableString;

namespace GGroupp.Infra;

internal static class DataverseHttpHelper
{
    private const string LoginMsOnlineServiceBaseUrl = "https://login.microsoftonline.com/";

    private static readonly JsonSerializerOptions jsonSerializerOptions;

    static DataverseHttpHelper()
        =>
        jsonSerializerOptions = new()
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

    internal static async Task<HttpClient> CreateHttpClientAsync(
        HttpMessageHandler messageHandler, 
        IDataverseApiClientConfiguration clientConfiguration,
        string apiVersion,
        string apiType,
        string? apiSearchType = null)
    {
        var authContext = CreateAuthenticationContext(clientConfiguration.AuthTenantId);
        var credential = new ClientCredential(clientConfiguration.AuthClientId, clientConfiguration.AuthClientSecret);

        var client = new HttpClient(messageHandler, disposeHandler: false)
        {
            BaseAddress = new(
                Invariant($"{clientConfiguration.ServiceUrl}/api/{apiType}/v{apiVersion}/{apiSearchType.OrEmpty()}"))
        };

        var authTokenResult = await authContext.AcquireTokenAsync(clientConfiguration.ServiceUrl, credential).ConfigureAwait(false);
        client.DefaultRequestHeaders.Authorization = new(authTokenResult.AccessTokenType, authTokenResult.AccessToken);

        return client;
    }

    private static AuthenticationContext CreateAuthenticationContext(string tenantId)
        =>
        new(LoginMsOnlineServiceBaseUrl + tenantId);

    public async static ValueTask<Result<T?, Failure<int>>> ReadDataverseResultAsync<T>(
        this HttpResponseMessage response, CancellationToken cancellationToken) 
        => 
        (response, await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false))switch
        {
            var (resp, body) when resp.IsSuccessStatusCode && typeof(T) == typeof(Unit)
                => default(T),
            var (resp, body) when resp.IsSuccessStatusCode
                => JsonSerializer.Deserialize<T>(body),
            var (resp, body) when resp.Content.Headers.ContentType?.MediaType != MediaTypeNames.Application.Json 
                => Failure.Create<int>(default, body),
            var (_, body) 
                => JsonSerializer.Deserialize<DataverseFailureJson>(body).Pipe(MapDataverseFailureJson)
        };
    
    private static Failure<int> MapDataverseFailureJson(DataverseFailureJson? failureJson)
        =>
        Pipeline.Pipe(
            failureJson?.Error?.Code)
        .Pipe(
            code => string.IsNullOrEmpty(code) ? default : Convert.ToInt32(code, 16))
        .Pipe(
            code => Failure.Create(code, failureJson?.Error?.Message));

    public static HttpContent BuildRequestJsonBody<TRequestJson>(TRequestJson input)
        =>
        Pipeline.Pipe(
            new StringContent(
                JsonSerializer.Serialize(input, jsonSerializerOptions),
                System.Text.Encoding.UTF8,
                MediaTypeNames.Application.Json))
        .Pipe(
            contetnt =>
            {
                contetnt.Headers.Add("Prefer", "return=representation");
                return contetnt;
            });

    public static DataverseSearchJsonIn MapDataverseSearchIn(this DataverseSearchIn input)
        =>
        new(input.Search)
        { 
            Entities = input.Entities,
            Facets = input.Facets,
            Filter = input.Filter,
            ReturnTotalRecordCount = input.ReturnTotalRecordCount,
            Skip = input.Skip,
            Top = input.Top,
            OrderBy = input.OrderBy,
            SearchMode = input.SearchMode?.ToDataverseSearchModeJson(),
            SearchType = input.SearchType?.ToDataverseSearchTypeJson()
        };

    public static DataverseSearchOut MapDataverseSearchJsonOut(this DataverseSearchJsonOut? @out)
       =>
       Pipeline.Pipe(
           @out?.Value?.Select(
                    item => new DataverseSearchItem(
                            searchScore: item.SearchScore,
                            entityName: item.EntityName,
                            objectId: item.ObjectId,
                            extensionData: MapJsonElementDictionary(item.ExtensionData ?? new())))
                        .ToArray())
        .Pipe(
           items => new DataverseSearchOut(@out?.TotalRecordCount ?? default, items));

    private static ReadOnlyDictionary<string, DataverseSearchJsonValue> MapJsonElementDictionary(
        Dictionary<string, JsonElement> jsonElemntDictonary)
        =>
        new(
            jsonElemntDictonary
            .ToDictionary<KeyValuePair<string,JsonElement>, string, DataverseSearchJsonValue>( 
                kv => kv.Key,
                kv => new(kv.Value)));
   

    private static DataverseSearchModeJson ToDataverseSearchModeJson(this DataverseSearchMode searchMode)
        =>
        searchMode switch
        {
            DataverseSearchMode.Any => DataverseSearchModeJson.Any,
            _ => DataverseSearchModeJson.All
        };

    private static DataverseSearchTypeJson ToDataverseSearchTypeJson(this DataverseSearchType searchType)
        =>
        searchType switch
        {
            DataverseSearchType.Simple => DataverseSearchTypeJson.Simple,
            _ => DataverseSearchTypeJson.Full
        };
}