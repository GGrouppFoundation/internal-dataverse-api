using System;
using System.Collections.Generic;
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

    internal static async Task<HttpClient> InternalCreateHttpClientAsync(
        HttpMessageHandler messageHandler, 
        DataverseApiClientConfiguration clientConfiguration,
        string apiVersion,
        string apiType,
        string? apiSearchType = null)
    {
        var authContext = CreateAuthenticationContext(clientConfiguration.AuthTenantId);
        var credential = new ClientCredential(clientConfiguration.AuthClientId, clientConfiguration.AuthClientSecret);

        using var client = new HttpClient(messageHandler, disposeHandler: false)
        {
            BaseAddress = new(
                Invariant($"{clientConfiguration.ServiceUrl}/api/{apiType}/v{apiVersion}/{apiSearchType.OrEmpty()}"))
        };

        var authTokenResult = await authContext.AcquireTokenAsync(clientConfiguration.ServiceUrl, credential).ConfigureAwait(false);
        client.DefaultRequestHeaders.Authorization = new(authTokenResult.AccessTokenType, authTokenResult.AccessToken);

        return client;
    }

    internal async static ValueTask<Result<T?, Failure<int>>> InternalReadDataverseResultAsync<T>(
        this HttpResponseMessage response, CancellationToken cancellationToken) 
        => 
        (response, await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false)) switch
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

    internal static HttpContent InternalBuildRequestJsonBody<TRequestJson>(TRequestJson input)
        =>
        new StringContent(
            JsonSerializer.Serialize(input, jsonSerializerOptions),
            System.Text.Encoding.UTF8,
            MediaTypeNames.Application.Json)
        .Pipe(
            contetnt =>
            {
                contetnt.Headers.Add("Prefer", "return=representation");
                return contetnt;
            });

    internal static DataverseSearchJsonIn InternalMapDataverseSearchIn(this DataverseSearchIn input)
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

    internal static DataverseSearchOut InternalMapDataverseSearchJsonOut(this DataverseSearchJsonOut? @out)
        =>
        new(
            @out?.TotalRecordCount ?? default,
            @out?.Value?.Select(MapJsonItem).ToArray());

    private static AuthenticationContext CreateAuthenticationContext(string tenantId)
        =>
        new(LoginMsOnlineServiceBaseUrl + tenantId);
    
    private static Failure<int> MapDataverseFailureJson(DataverseFailureJson failureJson)
        =>
        Pipeline.Pipe(
            failureJson.Error.Code)
        .Pipe(
            code => string.IsNullOrEmpty(code) ? default : Convert.ToInt32(code, 16))
        .Pipe(
            code => Failure.Create(code, failureJson.Error.Message));

    private static DataverseSearchItem MapJsonItem(DataverseSearchJsonItem jsonItem)
        =>
        new(
            searchScore: jsonItem.SearchScore,
            entityName: jsonItem.EntityName,
            objectId: jsonItem.ObjectId,
            extensionData: jsonItem.ExtensionData?.Select(MapJsonFieldValue).ToArray());

    private static KeyValuePair<string, DataverseSearchJsonValue> MapJsonFieldValue(KeyValuePair<string, JsonElement> source)
        =>
        new(
            key: source.Key,
            value: new(source.Value));
   

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