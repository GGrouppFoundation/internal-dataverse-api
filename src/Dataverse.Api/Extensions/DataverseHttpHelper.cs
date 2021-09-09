using System;
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

    public static async Task<HttpClient> CreateHttpClientAsync(
        HttpMessageHandler messageHandler, IDataverseApiClientConfiguration clientConfiguration)
    {
        var authContext = CreateAuthenticationContext(clientConfiguration.AuthTenantId);
        var credential = new ClientCredential(clientConfiguration.AuthClientId, clientConfiguration.AuthClientSecret);

        var client = new HttpClient(messageHandler, disposeHandler: false)
        {
            BaseAddress = new(
                $"{clientConfiguration.ServiceUrl}/api/{clientConfiguration.ApiType}/v{clientConfiguration.ApiVersion}/{clientConfiguration.ApiSearchType}")
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
                JsonSerializer.Serialize(
                    input, 
                    new JsonSerializerOptions() 
                    { 
                        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull 
                    }),
                System.Text.Encoding.UTF8,
                MediaTypeNames.Application.Json))
        .Pipe(contetnt =>
            {
                contetnt.Headers.Add("Prefer", "return=representation");

                return contetnt;
            });

    public static DataverseSearchJsonIn MapDataverseSearchIn(this DataverseSearchIn input)
        =>
        new DataverseSearchJsonIn(input.Search)
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
                DataverseSearchMode.all => DataverseSearchModeJson.all,
                DataverseSearchMode.any => DataverseSearchModeJson.any,
                null => null,
                _ => throw new ArgumentException(nameof(input.SearchMode))
            },
            SearchType = input.SearchType switch
            {
                DataverseSearchType.full => DataverseSearchTypeJson.full,
                DataverseSearchType.simple => DataverseSearchTypeJson.simple,
                null => null,
                _ => throw new ArgumentException(nameof(input.SearchType))
            }
        };

    public static DataverseSearchOut MapDataverseSearchJsonOut(this DataverseSearchJsonOut? @out)
       =>
       Pipeline.Pipe(
           @out?.Value?.Select(
                    item => new DataverseSearchItem(
                        item.SearchScore,
                        item.EntityName.OrEmpty(),
                        item.ObjectId)).ToArray() ?? Array.Empty<DataverseSearchItem>())
        .Pipe(
           items => new DataverseSearchOut(@out?.TotalRecordCount ?? default, items));
}