using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace GGroupp.Infra;

using IDataverseSearchJsonFieldValues = IReadOnlyCollection<KeyValuePair<string, DataverseSearchJsonValue>>;

public sealed record class DataverseSearchItem
{
    public DataverseSearchItem(
        double searchScore,
        Guid objectId,
        [AllowNull] string entityName,
        [AllowNull] IDataverseSearchJsonFieldValues extensionData)
    {
        SearchScore = searchScore;
        EntityName = entityName ?? string.Empty;    
        ObjectId = objectId;
        ExtensionData = extensionData ?? Array.Empty<KeyValuePair<string, DataverseSearchJsonValue>>();
    }

    public double SearchScore { get; }

    public Guid ObjectId { get; }

    public string EntityName { get; }

    public IDataverseSearchJsonFieldValues ExtensionData { get; }
}