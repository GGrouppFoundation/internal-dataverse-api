using System;

namespace GGroupp.Infra;

internal readonly record struct DataverseHttpRequest<TContent>
    where TContent : notnull
{
    private readonly string? url;

    public DataverseHttpRequest(DataverseHttpVerb verb, string url, FlatArray<DataverseHttpHeader> headers, Optional<TContent> content)
    {
        Verb = verb;
        this.url = url.OrNullIfEmpty();
        Headers = headers;
        Content = content;
    }

    public DataverseHttpVerb Verb { get; }

    public string? Url => url.OrEmpty();

    public FlatArray<DataverseHttpHeader> Headers { get; }

    public Optional<TContent> Content { get; }
}