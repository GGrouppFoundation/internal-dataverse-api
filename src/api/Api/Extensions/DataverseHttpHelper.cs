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
        DataverseApiClientConfiguration configuration,
        string apiVersion,
        string apiType,
        string? apiSearchType = null)
    {
        var authContext = CreateAuthenticationContext(configuration.AuthTenantId);
        var credential = new ClientCredential(configuration.AuthClientId, configuration.AuthClientSecret);

        var baseUri = new Uri(configuration.ServiceUrl, UriKind.Absolute);
        var client = new HttpClient(messageHandler, disposeHandler: false)
        {
            BaseAddress = new(baseUri, $"/api/{apiType}/v{apiVersion}/{apiSearchType.OrEmpty()}")
        };

        var authTokenResult = await authContext.AcquireTokenAsync(configuration.ServiceUrl, credential).ConfigureAwait(false);
        client.DefaultRequestHeaders.Authorization = new(authTokenResult.AccessTokenType, authTokenResult.AccessToken);

        return client;
    }

    internal async static ValueTask<Result<T?, Failure<DataverseFailureCode>>> InternalReadDataverseResultAsync<T>(
        this HttpResponseMessage response, CancellationToken cancellationToken)
    {
        if (response.IsSuccessStatusCode && typeof(T) == typeof(Unit))
        {
            return default(T);
        }

        var body = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
        if (response.IsSuccessStatusCode)
        {
            return JsonSerializer.Deserialize<T>(body);
        }

        if (response.Content.Headers.ContentType?.MediaType is not MediaTypeNames.Application.Json)
        {
            return Failure.Create<DataverseFailureCode>(default, body);
        }

        var failureJson = JsonSerializer.Deserialize<DataverseFailureJson>(body);

        if (failureJson.Failure is not null)
        {
            return CreateDataverseFailure(failureJson.Failure.Code, failureJson.Failure.Message ?? body);
        }

        if (failureJson.Error is not null)
        {
            return CreateDataverseFailure(failureJson.Error.Code, failureJson.Error.Description ?? body);
        }

        if (string.IsNullOrEmpty(failureJson.ErrorCode) is false)
        {
            return CreateDataverseFailure(failureJson.ErrorCode, failureJson.Message ?? failureJson.ExceptionMessage ?? body);
        }

        return Failure.Create(DataverseFailureCode.Unknown, body);
    }

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
    
    private static Failure<DataverseFailureCode> CreateDataverseFailure(string? code, string message)
        =>
        code switch
        {
            "0x80060891" or "0x80040217" => new(DataverseFailureCode.RecordNotFound, message),
            "0x8004431A" => new(DataverseFailureCode.PicklistValueOutOfRange, message),
            "0x80040220" => new(DataverseFailureCode.PrivilegeDenied, message),
            "0x80040225" => new(DataverseFailureCode.UserNotEnabled, message),
            "SearchableEntityNotFound" => new(DataverseFailureCode.SearchableEntityNotFound, message),
            "0x8005F103" or "0x80072322" or "0x80072326" or "0x80072321" or "0x80060308" => new(DataverseFailureCode.Throttling, message),
            _ => new(DataverseFailureCode.Unknown, message)
        };

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