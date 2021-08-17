#nullable enable

using System;
using System.Globalization;

namespace GGroupp.Infra
{
    public sealed record DataversePrimaryKey : IDataverseEntityKey
    {
        public DataversePrimaryKey(Guid entityId)
        {
            Value = entityId.ToString("D", CultureInfo.InvariantCulture);
        }

        public string Value { get; }
    }
}