using System;

namespace GarageGroup.Infra;

internal sealed record class DataverseJsonContentIn
{
    internal DataverseJsonContentIn(string json)
        =>
        Json = json.OrEmpty();

    public string Json { get; }
}