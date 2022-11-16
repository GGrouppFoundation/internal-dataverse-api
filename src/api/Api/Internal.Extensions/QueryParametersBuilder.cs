using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GGroupp.Infra;

internal static class QueryParametersBuilder
{
    internal static string BuildODataParameterValue(FlatArray<string> paramValues)
    {
        var valueBuilder = new StringBuilder();

        foreach (var paramValue in paramValues)
        {
            if (string.IsNullOrEmpty(paramValue))
            {
                continue;
            }

            if (valueBuilder.Length > 0)
            {
                valueBuilder.Append(',');
            }

            valueBuilder.Append(paramValue);
        }

        return valueBuilder.ToString();
    }

    internal static string BuildODataParameterValue(IEnumerable<string> paramValues)
    {
        var notEmptyValues = paramValues.Where(
            static v => string.IsNullOrEmpty(v) is false);
        
        return string.Join(',', notEmptyValues);
    }

    internal static string BuildQueryString(IReadOnlyCollection<KeyValuePair<string, string>> queryParams)
    {
        var queryStringBuilder = new StringBuilder();

        foreach(var queryParam in queryParams.Where(kv => string.IsNullOrEmpty(kv.Value) is false))
        {
            if(queryStringBuilder.Length > 0)
            {
                queryStringBuilder.Append('&');
            }
            else
            {
                queryStringBuilder.Append('?');
            }

            queryStringBuilder.Append(queryParam.Key).Append('=').Append(queryParam.Value);
        }

        return queryStringBuilder.ToString();
    }
}