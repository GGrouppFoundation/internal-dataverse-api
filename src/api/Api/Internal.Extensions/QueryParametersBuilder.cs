using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GGroupp.Infra;

internal static class QueryParametersBuilder
{
    internal static string BuildODataParameterValue(this FlatArray<string> paramValues)
    {
        if (paramValues.IsEmpty)
        {
            return string.Empty;
        }

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

    internal static string BuildQueryString(this IReadOnlyCollection<KeyValuePair<string, string>> queryParams)
    {
        if (queryParams.Count is not > 0)
        {
            return string.Empty;
        }

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
