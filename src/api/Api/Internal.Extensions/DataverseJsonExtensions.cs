using System;
using System.Text.Json;

namespace GarageGroup.Infra;

internal static class DataverseJsonExtensions
{
    static DataverseJsonExtensions()
        =>
        SerializerOptions = new(JsonSerializerDefaults.Web);

    private static readonly JsonSerializerOptions SerializerOptions;

    internal static DataverseJsonContentIn SerializeOrThrow<T>(this T value)
    {
        var type = value?.GetType() ?? typeof(T);
        var json = JsonSerializer.Serialize(value, type, SerializerOptions);

        return new DataverseJsonContentIn(json.OrEmpty());
    }

    internal static T? DeserializeOrThrow<T>(this DataverseJsonContentOut? content)
    {
        if (string.IsNullOrEmpty(content?.Json))
        {
            return default;
        }

        return JsonSerializer.Deserialize<T>(content.Json, SerializerOptions);
    }
}
