using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Xml;

namespace GGroupp.Infra;

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
        DataverseFetchXmlIn input, CancellationToken cancellationToken = default)
    {
        var request = new DataverseHttpRequest<Unit>(
            verb: DataverseHttpVerb.Get,
            url: BuildFetchXmlUri(input),
            headers: GetHeaders(),
            content: default);

        var result = await httpApi.InvokeAsync<Unit, DataverseFetchXmlOutJson<TEntityJson>>(request, cancellationToken).ConfigureAwait(false);
        return result.MapSuccess(MapSuccess);

        static DataverseFetchXmlOut<TEntityJson> MapSuccess(DataverseFetchXmlOutJson<TEntityJson> success)
        {
            var pagingCookie = success.PagingCookie;
            
            if (string.IsNullOrEmpty(pagingCookie) is false)
            {
                var xmlPagingCookie = new XmlDocument();
                xmlPagingCookie.LoadXml(pagingCookie);
                
                var innerCookie = xmlPagingCookie.DocumentElement?.Attributes.GetNamedItem(PagingCookieAttributeName)?.Value;
                var decodedPagingCookie = WebUtility.UrlDecode(WebUtility.UrlDecode(innerCookie));
                
                var htmlEncodedPagingCookie = WebUtility.HtmlEncode(decodedPagingCookie);
                pagingCookie = WebUtility.UrlEncode(htmlEncodedPagingCookie);
            }
            
            return new(success.Value, pagingCookie);
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