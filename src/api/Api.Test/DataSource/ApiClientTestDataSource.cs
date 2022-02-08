using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GGroupp.Infra.Dataverse.Api.Test;

internal static partial class ApiClientTestDataSource
{
    private static StringContent CreateResponseContentJson<TJson>(TJson responseJson)
        =>
        new(
            Serialize(responseJson));

    private static string Serialize<T>(T value)
        =>
        JsonSerializer.Serialize(
            value,
            new JsonSerializerOptions
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            });
}