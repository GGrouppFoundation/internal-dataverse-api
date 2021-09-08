using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace GGroupp.Infra;

public sealed record DataverseAlternateKey : IDataverseEntityKey
{
    public string Value { get; }

    public DataverseAlternateKey(IReadOnlyCollection<KeyValuePair<string, string>> idArguments)
    {
        var args = idArguments?.Where(kv => string.IsNullOrEmpty(kv.Value) is false) ?? Array.Empty<KeyValuePair<string, string>>();
        Value = BuildIdArgs(args);
    }

    private static string BuildIdArgs(IEnumerable<KeyValuePair<string, string>> args)
        =>
        string.Join(',', args.Select(kv => WebUtility.UrlEncode($"{kv.Key}={kv.Value}")));
}