#nullable enable

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace GGroupp.Infra
{
    public sealed record DataverseEntityGetIn
    {
        public DataverseEntityGetIn(
            [AllowNull] string entityPluralName,
            Guid entityId,
            [AllowNull] IReadOnlyCollection<string> selectFields)
        {
            EntityPluralName = entityPluralName ?? string.Empty;
            SelectFields = selectFields ?? Array.Empty<string>();
            EntityId = entityId;
        }

        public string EntityPluralName { get; }

        public IReadOnlyCollection<string> SelectFields { get; }

        public Guid EntityId { get; }
    }
}