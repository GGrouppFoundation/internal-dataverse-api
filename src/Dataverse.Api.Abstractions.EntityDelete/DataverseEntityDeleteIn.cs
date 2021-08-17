#nullable enable

using System;
using System.Diagnostics.CodeAnalysis;

namespace GGroupp.Infra
{
    public sealed record DataverseEntityDeleteIn
    {
        public DataverseEntityDeleteIn(
            [AllowNull] string entityPluralName,
            IDataverseEntityKey entityKey)
        {
            EntityKey = entityKey;
            EntityPluralName = entityPluralName ?? string.Empty;
        }

        public string EntityPluralName { get; }

        public IDataverseEntityKey EntityKey{ get; }
    }
}