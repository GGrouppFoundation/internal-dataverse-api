#nullable enable

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace GGroupp.Infra
{
    public sealed record DataverseEntitySetGetIn
    {
        public DataverseEntitySetGetIn(
            [AllowNull] string entitySetName,
            [AllowNull] IReadOnlyCollection<string> selectFields,
            [AllowNull] string filter)
        {
            EntitySetName = entitySetName ?? string.Empty;
            SelectFields = selectFields ?? Array.Empty<string>();
            Filter = filter ?? string.Empty;
        }

        public string EntitySetName { get; }

        public IReadOnlyCollection<string> SelectFields { get; }

        public string Filter { get; }
    }
}