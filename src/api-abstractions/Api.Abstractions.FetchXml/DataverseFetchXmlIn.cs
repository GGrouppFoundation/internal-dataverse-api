namespace GGroupp.Infra;

public sealed record class DataverseFetchXmlIn
{
    public string FetchXmlQueryString { get; }
    
    public string EntityPluralName { get; }
    
    public string? IncludeAnnotations { get; init; }

    public DataverseFetchXmlIn(string entityPluralName, string fetchXmlQueryString)
    {
        FetchXmlQueryString = fetchXmlQueryString ?? string.Empty;
        EntityPluralName = entityPluralName ?? string.Empty;
    }
}