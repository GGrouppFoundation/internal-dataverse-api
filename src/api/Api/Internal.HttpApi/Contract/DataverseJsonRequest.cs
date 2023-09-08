using System;
using System.Diagnostics.CodeAnalysis;

namespace GarageGroup.Infra;

internal sealed record class DataverseJsonRequest
{
    public DataverseJsonRequest(
        DataverseHttpVerb verb,
        [AllowNull] string url,
        FlatArray<DataverseHttpHeader> headers,
        DataverseJsonContentIn? content)
    {
        Verb = verb;
        Url = url.OrNullIfEmpty();
        Headers = headers;
        Content = content;
    }

    public DataverseHttpVerb Verb { get; }

    public string? Url { get; }

    public FlatArray<DataverseHttpHeader> Headers { get; }

    public DataverseJsonContentIn? Content { get; }
}