using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;

namespace GGroupp.Infra;

public sealed record DataverseSearchItem
{
    public DataverseSearchItem(
        double searchScore,
        [AllowNull] string entityName,
        Guid objectId,
        [AllowNull] ReadOnlyDictionary<string, DataverseSearchJsonValue> extensionData)
    {
        SearchScore = searchScore;
        EntityName = entityName ?? string.Empty;    
        ObjectId = objectId;
        ExtensionData = extensionData;
    }

    public double SearchScore { get; }

    public string EntityName { get; }

    public Guid ObjectId { get; }

    public IReadOnlyDictionary<string, DataverseSearchJsonValue>? ExtensionData { get; }
}