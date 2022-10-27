using System;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace GGroupp.Infra;

internal static class DataverseHttpHelper
{
    private static readonly JsonSerializerOptions jsonSerializerOptions;

    static DataverseHttpHelper()
        =>
        jsonSerializerOptions = new()
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

    internal static HttpRequestMessage IncludeAnnotationsHeaderValue(this HttpRequestMessage requestMessage, string? includeAnnotations)
    {
        if (string.IsNullOrEmpty(includeAnnotations))
        {
            return requestMessage;
        }

        requestMessage.Headers.TryAddWithoutValidation("Prefer", $"odata.include-annotations={includeAnnotations}");
        return requestMessage;
    }

    internal async static ValueTask<Result<T?, Failure<DataverseFailureCode>>> ReadDataverseResultAsync<T>(
        this HttpResponseMessage response, CancellationToken cancellationToken)
    {
        if (response.IsSuccessStatusCode && typeof(T) == typeof(Unit))
        {
            return default(T);
        }

        var body = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
        if (response.IsSuccessStatusCode)
        {
            if (string.IsNullOrEmpty(body))
            {
                return default(T);
            }

            return JsonSerializer.Deserialize<T>(body);
        }

        var failureJson = DeserializeFailure(body: body, mediaType: response.Content.Headers.ContentType?.MediaType);
        return failureJson.ToDataverseFailure(body, response.StatusCode);
    }

    internal static HttpContent BuildRequestJsonBody<TRequestJson>(TRequestJson input)
    {
        var json = JsonSerializer.Serialize(input, jsonSerializerOptions);
        var content = new StringContent(json, Encoding.UTF8, MediaTypeNames.Application.Json);

        content.Headers.Add("Prefer", "return=representation");
        return content;
    }

    private static DataverseFailureJson? DeserializeFailure(string? body, string? mediaType)
    {
        if (mediaType is MediaTypeNames.Application.Json && string.IsNullOrEmpty(body) is false)
        {
            return JsonSerializer.Deserialize<DataverseFailureJson>(body);
        }

        return null;
    }

    private static Failure<DataverseFailureCode> ToDataverseFailure(
        this DataverseFailureJson? failureJson, string? body, HttpStatusCode statusCode)
    {
        if (failureJson is null)
        {
            return new(GetFailureCode(null), GetFailureMessage(null));
        }

        var value = failureJson.Value;

        if (value.Failure is not null)
        {
            return new(GetFailureCode(value.Failure.Code), GetFailureMessage(value.Failure.Message));
        }

        if (value.Error is not null)
        {
            return new(GetFailureCode(value.Error.Code), GetFailureMessage(value.Error.Description));
        }

        var failureCode = GetFailureCode(value.ErrorCode);

        if (string.IsNullOrEmpty(value.Message) is false)
        {
            return new(failureCode, value.Message);
        }

        return new(failureCode, GetFailureMessage(value.ExceptionMessage));

        string GetFailureMessage(string? message)
        {
            if (string.IsNullOrEmpty(message) is false)
            {
                return message;
            }

            if (string.IsNullOrEmpty(body) is false)
            {
                return body;
            }

            return $"Dataverse respose status was {statusCode}";
        }

        DataverseFailureCode GetFailureCode(string? code)
            =>
            statusCode.ToDataverseFailureCode(code);
    }

    private static DataverseFailureCode ToDataverseFailureCode(this HttpStatusCode statusCode, string? code)
    {
        if (statusCode is HttpStatusCode.Unauthorized)
        {
            return DataverseFailureCode.Unauthorized;
        }

        return code switch
        {
            "0x80060891" or "0x80040217" => DataverseFailureCode.RecordNotFound,
            "0x8004431A" => DataverseFailureCode.PicklistValueOutOfRange,
            "0x80040220" or "0x80048306" => DataverseFailureCode.PrivilegeDenied,
            "0x80040225" or "0x8004d24b" => DataverseFailureCode.UserNotEnabled,
            "SearchableEntityNotFound" => DataverseFailureCode.SearchableEntityNotFound,
            "0x8005F103" or "0x80072322" or "0x80072326" or "0x80072321" or "0x80060308" => DataverseFailureCode.Throttling,
            _ => DataverseFailureCode.Unknown
        };
    }
}