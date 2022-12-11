using System;

namespace GGroupp.Infra;

public sealed record class DataverseSearchIn
{
    public DataverseSearchIn(string searchString)
        => 
        Search = searchString ?? string.Empty;

    public string Search { get; }

    public FlatArray<string>? Entities { get; init; }

    public FlatArray<string>? Facets { get; init; }

    public string? Filter { get; init; }

    public bool? ReturnTotalRecordCount { get; init; }

    public int? Skip { get; init; }

    public int? Top { get; init; }

    public FlatArray<string>? OrderBy { get; init; }

    public DataverseSearchMode? SearchMode {  get; init; }

    public DataverseSearchType? SearchType { get; init; }
}