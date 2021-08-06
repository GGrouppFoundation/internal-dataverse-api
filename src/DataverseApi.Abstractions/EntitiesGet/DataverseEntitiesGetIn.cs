#nullable enable

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace GGroupp
{
    public sealed record DataverseEntitiesGetIn
    {
        public DataverseEntitiesGetIn(
            [AllowNull] string entitiesName,
            [AllowNull] IReadOnlyCollection<string> selectFields,
            [AllowNull] string filter)
        {
            EntitiesName = entitiesName ?? string.Empty;
            SelectFields = selectFields ?? Array.Empty<string>();
            Filter = filter ?? string.Empty;
        }

        public string EntitiesName { get; }

        public IReadOnlyCollection<string> SelectFields { get; }

        public string Filter { get; }
    }
}