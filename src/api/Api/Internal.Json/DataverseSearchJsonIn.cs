using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace GGroupp.Infra;

internal readonly record struct DataverseSearchJsonIn
{
    [JsonPropertyName("search")]
    public required string? Search { get; init; }

    [JsonPropertyName("entities")]
    public FlatArray<string>? Entities { get; init; }

    [JsonPropertyName("facets")]
    public FlatArray<string>? Facets { get; init; }

    [JsonPropertyName("filter")]
    public string? Filter { get; init; }

    [JsonPropertyName("returntotalrecordcount")]
    public bool? ReturnTotalRecordCount { get; init; }

    [JsonPropertyName("skip")]
    public int? Skip { get; init; }

    [JsonPropertyName("top")]
    public int? Top { get; init; }

    [JsonPropertyName("orderby")]
    public FlatArray<string>? OrderBy { get; init; }

    [JsonPropertyName("searchmode")]
    public DataverseSearchModeJson? SearchMode { get; init; }

    [JsonPropertyName("searchtype")]
    public DataverseSearchTypeJson? SearchType { get; init; }
}