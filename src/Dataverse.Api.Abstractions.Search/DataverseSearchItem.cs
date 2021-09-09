using System.Text.Json.Serialization;

namespace GGroupp.Infra;

public sealed record DataverseSearchItem
{
    public DataverseSearchItem(
        double searchScore,
        string entityName,
        Guid objectId)
    {
        SearchScore = searchScore;
        EntityName = entityName;    
        ObjectId = objectId;
    }

    public double SearchScore { get; }

    public string EntityName { get; }

    public Guid ObjectId { get; }
}