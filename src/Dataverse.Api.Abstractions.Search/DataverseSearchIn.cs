using System.Text.Json.Serialization;

namespace GGroupp.Infra;

public sealed record DataverseSearchIn
{
    public DataverseSearchIn(string searchString)
        => Search = searchString;

    public string Search { get; }

    public IReadOnlyCollection<string>? Entities { get; init; }

    public IReadOnlyCollection<string>? Facets { get; init; }

    public string? Filter { get; init; }

    public bool? ReturnTotalRecordCount { get; init; }

    public int? Skip { get; init; }

    public int? Top { get; init; }

    public IReadOnlyCollection<string>? OrderBy { get; init; }

    public DataverseSearchMode? SearchMode {  get; init; }

    public DataverseSearchType? SearchType { get; init; }
}