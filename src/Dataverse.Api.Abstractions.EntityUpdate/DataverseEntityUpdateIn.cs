#nullable enable

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace GGroupp.Infra
{
    public sealed record DataverseEntityUpdateIn<TRequestJson>
    {
        public DataverseEntityUpdateIn(
            [AllowNull] string entityPluralName,
            IDataverseEntityKey entityId,
            [AllowNull] IReadOnlyCollection<string> selectFields,
            TRequestJson entityData)
        {
            EntityPluralName = entityPluralName ?? string.Empty;
            EntityId = entityId;
            SelectFields = selectFields ?? Array.Empty<string>();
            EntityData = entityData;
        }

        public string EntityPluralName { get; }

        public IReadOnlyCollection<string> SelectFields { get; }

        public TRequestJson EntityData { get; }

        public IDataverseEntityKey EntityId { get; }
    }
}