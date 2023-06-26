using System;
using System.Text.Json.Serialization;

namespace GarageGroup.Infra;

internal readonly record struct DataverseSearchJsonIn
{
    [JsonPropertyName("search")]
    public string? Search { get; init; }

    [JsonPropertyName("entities")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public FlatArray<string>? Entities { get; init; }

    [JsonPropertyName("facets")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public FlatArray<string>? Facets { get; init; }

    [JsonPropertyName("filter")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Filter { get; init; }

    [JsonPropertyName("returntotalrecordcount")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? ReturnTotalRecordCount { get; init; }

    [JsonPropertyName("skip")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? Skip { get; init; }

    [JsonPropertyName("top")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? Top { get; init; }

    [JsonPropertyName("orderby")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public FlatArray<string>? OrderBy { get; init; }

    [JsonPropertyName("searchmode")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public DataverseSearchModeJson? SearchMode { get; init; }

    [JsonPropertyName("searchtype")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public DataverseSearchTypeJson? SearchType { get; init; }
}