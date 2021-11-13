using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace GGroupp.Infra;

internal sealed record class DataverseSearchJsonOut
{
    [JsonPropertyName("totalrecordcount")]
    public int TotalRecordCount { get; init; }

    [JsonPropertyName("value")]
    public IReadOnlyCollection<DataverseSearchJsonItem>? Value {  get; init; }   
}
