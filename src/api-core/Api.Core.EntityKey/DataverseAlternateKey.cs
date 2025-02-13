using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace GarageGroup.Infra;

public sealed record class DataverseAlternateKey : IDataverseEntityKey
{
    public DataverseAlternateKey(params IReadOnlyCollection<KeyValuePair<string, string>> idArguments)
        =>
        Value = BuildValue(idArguments);

    public DataverseAlternateKey(string fieldName, string fieldValue)
        =>
        Value = BuildAlternateKeyItem(fieldName ?? string.Empty, fieldValue ?? string.Empty);

    public string Value { get; }

    private static string BuildValue([AllowNull] IReadOnlyCollection<KeyValuePair<string, string>> idArguments)
    {
        if (idArguments?.Count is not > 0)
        {
            return string.Empty;
        }

        return string.Join(',', idArguments.Where(IsValueNotEmpty).Select(BuildValueItem));

        static bool IsValueNotEmpty(KeyValuePair<string, string> kv)
            =>
            string.IsNullOrEmpty(kv.Value) is false;

        static string BuildValueItem(KeyValuePair<string, string> kv)
            =>
            BuildAlternateKeyItem(kv.Key, kv.Value);
    }

    private static string BuildAlternateKeyItem(string key, string value)
        =>
        $"{key}={value}";
}