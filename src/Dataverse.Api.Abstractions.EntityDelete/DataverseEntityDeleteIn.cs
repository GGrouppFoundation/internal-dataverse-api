#nullable enable

using System;
using System.Diagnostics.CodeAnalysis;

namespace GGroupp.Infra
{
    public sealed record DataverseEntityDeleteIn
    {
        public DataverseEntityDeleteIn(
            [AllowNull] string entityPluralName,
            Guid entityId)
        {
            EntityId = entityId;
            EntityPluralName = entityPluralName ?? string.Empty;
        }

        public string EntityPluralName { get; }

        public Guid EntityId { get; }
    }
}