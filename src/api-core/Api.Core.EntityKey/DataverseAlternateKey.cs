using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace GGroupp.Infra;

public sealed record class DataverseAlternateKey : IDataverseEntityKey
{
    public DataverseAlternateKey(IReadOnlyCollection<KeyValuePair<string, string>> idArguments)
    {
        var args = idArguments?.Where(kv => string.IsNullOrEmpty(kv.Value) is false);
        Value = BuildValue(args ?? Enumerable.Empty<KeyValuePair<string, string>>());
    }

    public string Value { get; }

    private static string BuildValue(IEnumerable<KeyValuePair<string, string>> args)
        =>
        string.Join(',', args.Select(kv => WebUtility.UrlEncode($"{kv.Key}={kv.Value}")));
}