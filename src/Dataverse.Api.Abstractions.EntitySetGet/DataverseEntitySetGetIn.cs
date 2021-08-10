#nullable enable

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace GGroupp.Infra
{
    public sealed record DataverseEntitySetGetIn
    {
        public DataverseEntitySetGetIn(
            [AllowNull] string entityPluralName,
            [AllowNull] IReadOnlyCollection<string> selectFields,
            [AllowNull] string filter)
        {
            EntityPluralName = entityPluralName ?? string.Empty;
            SelectFields = selectFields ?? Array.Empty<string>();
            Filter = filter ?? string.Empty;
        }

        public string EntityPluralName { get; }

        public IReadOnlyCollection<string> SelectFields { get; }

        public string Filter { get; }
    }
}