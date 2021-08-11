#nullable enable

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace GGroupp.Infra
{
    public sealed record DataverseEntityCreateIn<TRequestJson>
    {
        public DataverseEntityCreateIn(
            [AllowNull] string entityPluralName,
            [AllowNull] IReadOnlyCollection<string> selectFields,
            TRequestJson requestJson
            )
        {
            EntityPluralName = entityPluralName ?? string.Empty;
            SelectFields = selectFields ?? Array.Empty<string>();
            RequestJson = requestJson;
        }

        public string EntityPluralName { get; }

        public IReadOnlyCollection<string> SelectFields { get; }

        public TRequestJson RequestJson { get; }
    }
}