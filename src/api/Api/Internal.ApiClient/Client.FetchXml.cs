using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Xml;

namespace GarageGroup.Infra;

internal sealed partial class DataverseApiClient
{
    public ValueTask<Result<DataverseFetchXmlOut<TEntityJson>, Failure<DataverseFailureCode>>> FetchXmlAsync<TEntityJson>(
        DataverseFetchXmlIn input, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(input);

        if (cancellationToken.IsCancellationRequested)
        {
            return GetCanceledAsync<DataverseFetchXmlOut<TEntityJson>>(cancellationToken);
        }

        return InnerFetchXmlAsync<TEntityJson>(input, cancellationToken);
    }

    private async ValueTask<Result<DataverseFetchXmlOut<TEntityJson>, Failure<DataverseFailureCode>>> InnerFetchXmlAsync<TEntityJson>(
        DataverseFetchXmlIn input, CancellationToken cancellationToken)
    {
        try
        {
            var request = new DataverseJsonRequest(
                verb: DataverseHttpVerb.Get,
                url: BuildFetchXmlUri(input),
                headers: GetHeaders(),
                content: default);

            var result = await httpApi.SendJsonAsync(request, cancellationToken).ConfigureAwait(false);
            return result.MapSuccess(MapSuccess);
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            return ToDataverseFailure(ex, "An unexpected exception was thrown when trying to fetch Dataverse entities");
        }

        static DataverseFetchXmlOut<TEntityJson> MapSuccess(DataverseJsonResponse response)
        {
            var content = response.Content.DeserializeOrThrow<DataverseFetchXmlOutJson<TEntityJson>>();
            var pagingCookie = content.PagingCookie;

            if (string.IsNullOrEmpty(pagingCookie) is false)
            {
                var xmlPagingCookie = new XmlDocument();
                xmlPagingCookie.LoadXml(pagingCookie);

                var innerCookie = xmlPagingCookie.DocumentElement?.Attributes.GetNamedItem(PagingCookieAttributeName)?.Value;
                var decodedPagingCookie = WebUtility.UrlDecode(WebUtility.UrlDecode(innerCookie));

                var htmlEncodedPagingCookie = WebUtility.HtmlEncode(decodedPagingCookie);
                pagingCookie = WebUtility.UrlEncode(htmlEncodedPagingCookie);
            }

            return new(content.Value, pagingCookie);
        }

        FlatArray<DataverseHttpHeader> GetHeaders()
        {
            var preferValue = BuildPreferValue(input.IncludeAnnotations);

            if (string.IsNullOrEmpty(preferValue))
            {
                return GetAllHeaders();
            }

            return GetAllHeaders(
                new DataverseHttpHeader(PreferHeaderName, preferValue));
        }
    }
    
    private static string BuildFetchXmlUri(DataverseFetchXmlIn input)
        =>
        BuildDataRequestUrl($"{HttpUtility.UrlEncode(input.EntityPluralName)}?fetchXml={input.FetchXmlQueryString}");
}