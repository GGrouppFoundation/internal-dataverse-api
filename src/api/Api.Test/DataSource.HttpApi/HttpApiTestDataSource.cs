using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GGroupp.Infra.Dataverse.Api.Test;

internal static partial class HttpApiTestDataSource
{
    private static StringContent CreateResponseContentJson<TJson>(this TJson responseJson)
        =>
        new(
            Serialize(responseJson));

    private static StringContent ToJsonContent(this StubFailureJson failureJson)
        =>
        new(failureJson.Serialize(), default, MediaTypeNames.Application.Json);

    private static string Serialize<T>(this T value)
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