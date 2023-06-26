namespace GarageGroup.Infra;

public sealed record class DataverseFetchXmlIn
{
    public DataverseFetchXmlIn(string entityPluralName, string fetchXmlQueryString)
    {
        FetchXmlQueryString = fetchXmlQueryString ?? string.Empty;
        EntityPluralName = entityPluralName ?? string.Empty;
    }

    public string FetchXmlQueryString { get; }
    
    public string EntityPluralName { get; }
    
    public string? IncludeAnnotations { get; init; }
}