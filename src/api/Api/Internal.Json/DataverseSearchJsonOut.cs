using System;
using System.Text.Json.Serialization;

namespace GarageGroup.Infra;

internal readonly record struct DataverseSearchJsonOut
{
    [JsonPropertyName("totalrecordcount")]
    public int TotalRecordCount { get; init; }

    [JsonPropertyName("value")]
    public FlatArray<DataverseSearchJsonItem> Value { get; init; }
}
