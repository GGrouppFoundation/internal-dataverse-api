#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace GGroupp.Infra
{
    public sealed record DataverseEntityKey : IDataverseEntityKey
    {
        public DataverseEntityKey(Guid entityId)
        {
            Value = entityId.ToString();
        }

        public string Value { get; }

        public DataverseEntityKey(IReadOnlyCollection<KeyValuePair<string, string>> idArguments)
        {
            var args = idArguments?.Where(kv => string.IsNullOrEmpty(kv.Value) is false) ?? Array.Empty<KeyValuePair<string, string>>();
            Value = BuildIdArgs(args);
        }

        public static string BuildIdArgs(IEnumerable<KeyValuePair<string, string>> args)
            =>
            string.Join(',', args.Select(kv => WebUtility.UrlEncode($"{kv.Key}={kv.Value}")));
    }
}