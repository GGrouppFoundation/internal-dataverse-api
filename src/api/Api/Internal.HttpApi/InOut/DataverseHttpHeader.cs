using System;
using System.Diagnostics.CodeAnalysis;

namespace GGroupp.Infra;

internal sealed record class DataverseHttpHeader
{
    public DataverseHttpHeader(string name, [AllowNull] string value)
    {
        Name = name.OrEmpty();
        Value = value.OrNullIfEmpty();
    }

    public string Name { get; }

    public string? Value { get; }
}