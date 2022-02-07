using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GGroupp.Infra;

internal static class QueryParametersBuilder
{
    internal static string BuildOdataParameterValue(IReadOnlyCollection<string> paramValues)
        =>
        paramValues.Where(
            v => string.IsNullOrEmpty(v) is false)
        .Pipe(
            values => string.Join(',', values));

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