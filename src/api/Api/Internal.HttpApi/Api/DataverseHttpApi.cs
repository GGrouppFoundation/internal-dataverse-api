using System;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GGroupp.Infra;

internal sealed partial class DataverseHttpApi : IDataverseHttpApi
{
    static DataverseHttpApi()
        =>
        SerializerOptions = new()
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

    private static readonly JsonSerializerOptions SerializerOptions;

    private readonly HttpMessageHandler messageHandler;

    private readonly Uri dataverseBaseUri;

    internal DataverseHttpApi(HttpMessageHandler messageHandler, Uri dataverseBaseUri)
    {
        this.messageHandler = messageHandler;
        this.dataverseBaseUri = dataverseBaseUri;
    }

    private HttpClient CreateHttpClient(string? relativeUrl)
        =>
        new(messageHandler, disposeHandler: false)
        {
            BaseAddress = new(dataverseBaseUri, relativeUrl)
        };

    private static HttpContent? BuildRequestJsonBody<TRequestJson>(TRequestJson input)
        where TRequestJson : notnull
    {
        var json = JsonSerializer.Serialize(input, SerializerOptions);
        return new StringContent(json, Encoding.UTF8, MediaTypeNames.Application.Json);
    }

    private static TOut? DeserializeSuccess<TOut>(string? body)
    {
        if (string.IsNullOrEmpty(body))
        {
            return default;
        }

        return JsonSerializer.Deserialize<TOut>(body, SerializerOptions);
    }

    private static DataverseFailureJson? DeserializeFailure(string? body, string? mediaType)
    {
        if (mediaType is MediaTypeNames.Application.Json && string.IsNullOrEmpty(body) is false)
        {
            return JsonSerializer.Deserialize<DataverseFailureJson>(body, SerializerOptions);
        }

        return null;
    }

    private static Failure<DataverseFailureCode> ToDataverseFailure(
        DataverseFailureJson? failureJson, string? body, HttpStatusCode statusCode)
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
            ToDataverseFailureCode(statusCode, code);
    }

    private static DataverseFailureCode ToDataverseFailureCode(HttpStatusCode statusCode, string? code)
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
            "0x80040333" or "0x80060892" => DataverseFailureCode.DuplicateRecord,
            _ => DataverseFailureCode.Unknown
        };
    }
}