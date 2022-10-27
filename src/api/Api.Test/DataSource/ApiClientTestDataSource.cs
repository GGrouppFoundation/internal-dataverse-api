using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GGroupp.Infra.Dataverse.Api.Test;

internal static partial class ApiClientTestDataSource
{
    private static StringContent CreateResponseContentJson<TJson>(TJson responseJson)
        =>
        new(
            Serialize(responseJson));

    private static StringContent ToJsonContent(this DataverseFailureJson failureJson)
        =>
        new(failureJson.Serialize(), default, MediaTypeNames.Application.Json);

    private static string Serialize(this DataverseFailureJson failureJson)
        =>
        JsonSerializer.Serialize(failureJson);

    private static string Serialize<T>(T value)
        =>
        JsonSerializer.Serialize(
            value,
            new JsonSerializerOptions
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            });

    private static string GetDefaultFailureMessage(this HttpStatusCode statusCode)
        =>
        $"Dataverse respose status was {statusCode}";
}