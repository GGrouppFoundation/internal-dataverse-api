using System.Text.Json.Serialization;

namespace GGroupp.Infra.Dataverse.Api.Test;

internal sealed record class StubSearchJsonIn
{
    [JsonPropertyName("search")]
    public string? Search { get; init; }

    [JsonPropertyName("entities")]
    public string[]? Entities { get; init; }

    [JsonPropertyName("facets")]
    public string[]? Facets { get; init; }

    [JsonPropertyName("filter")]
    public string? Filter { get; init; }

    [JsonPropertyName("returntotalrecordcount")]
    public bool? ReturnTotalRecordCount { get; init; }

    [JsonPropertyName("skip")]
    public int? Skip { get; init; }

    [JsonPropertyName("top")]
    public int? Top { get; init; }

    [JsonPropertyName("orderby")]
    public string[]? OrderBy { get; init; }

    [JsonPropertyName("searchmode")]
    public int? SearchMode { get; init; }

    [JsonPropertyName("searchtype")]
    public int? SearchType { get; init; }
}