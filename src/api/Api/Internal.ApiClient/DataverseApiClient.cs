using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Infra;

internal sealed partial class DataverseApiClient : IDataverseApiClient
{
    private const string ApiVersionData = "9.2";

    private const string ApiTypeData = "data";

    private const string WhoAmIRelativeUrl = "WhoAmI";

    private const string CallerIdHeaderName = "MSCRMCallerID";

    private const string PreferHeaderName = "Prefer";

    private const string ReturnRepresentationValue = "return=representation";

    private const string SuppressDuplicateDetectionHeaderName = "MSCRM.SuppressDuplicateDetection";

    private const string SearchRequestUrl = "/api/search/v1.0/query";

    private const string PagingCookieAttributeName = "pagingcookie";

    private readonly IDataverseHttpApi httpApi;

    private readonly Guid? callerId;

    internal DataverseApiClient(IDataverseHttpApi httpApi)
        =>
        this.httpApi = httpApi;

    private DataverseApiClient(IDataverseHttpApi httpApi, Guid callerId)
        =>
        (this.httpApi, this.callerId) = (httpApi, callerId);

    private static string BuildDataRequestUrl(string dataUrl)
        =>
        $"/api/{ApiTypeData}/v{ApiVersionData}/{dataUrl}";

    private FlatArray<DataverseHttpHeader> GetAllHeadersWithRepresentation(bool? suppressDuplicateDetection)
    {
        var preferHeader = new DataverseHttpHeader(PreferHeaderName, ReturnRepresentationValue);
        if (suppressDuplicateDetection is null)
        {
            return GetAllHeaders(preferHeader);
        }

        return GetAllHeaders(
            preferHeader,
            GetSuppressDuplicateDetectionHeader(suppressDuplicateDetection.Value));
    }

    private FlatArray<DataverseHttpHeader> GetAllHeadersWithoutRepresentation(bool? suppressDuplicateDetection)
    {
        if (suppressDuplicateDetection is null)
        {
            return GetAllHeaders();
        }

        return GetAllHeaders(
            GetSuppressDuplicateDetectionHeader(suppressDuplicateDetection.Value));
    }

    private FlatArray<DataverseHttpHeader> GetAllHeaders(params DataverseHttpHeader[] headers)
    {
        if (callerId is null && headers.Length is 0)
        {
            return default;
        }

        if (callerId is null)
        {
            return new(headers);
        }

        var array = new DataverseHttpHeader[headers.Length + 1];
        array[0] = new(CallerIdHeaderName, callerId.Value.ToString("D"));

        for (var i = 0; i < headers.Length; i++)
        {
            array[i + 1] = headers[i];
        }

        return array;
    }

    private static DataverseHttpHeader GetSuppressDuplicateDetectionHeader(bool suppressDuplicateDetection)
        =>
        new(
            SuppressDuplicateDetectionHeaderName,
            GetHeaderValue(suppressDuplicateDetection));

    private static string BuildPreferValue(string? includeAnnotations, int? maxPageSize = null)
    {
        return string.Join(',', GetPreferValues());

        IEnumerable<string> GetPreferValues()
        {
            if (maxPageSize is not null)
            {
                yield return $"odata.maxpagesize={maxPageSize}";
            }

            if (string.IsNullOrEmpty(includeAnnotations) is false)
            {
                yield return $"odata.include-annotations={includeAnnotations}";
            }
        }
    }

    private static string GetHeaderValue(bool value)
        =>
        value ? "true" : "false";

    private static ValueTask<Result<T, Failure<DataverseFailureCode>>> GetCanceledAsync<T>(CancellationToken cancellationToken)
        =>
        ValueTask.FromCanceled<Result<T, Failure<DataverseFailureCode>>>(cancellationToken);
}