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

    internal static string BuildExpandFieldValue(this DataverseExpandedField expandedField)
    {
        if (string.IsNullOrEmpty(expandedField.FieldName))
        {
            return string.Empty;
        }

        if (expandedField.SelectFields.IsEmpty && expandedField.ExpandFields.IsEmpty)
        {
            return expandedField.FieldName;
        }

        var valueBuilder = new StringBuilder(expandedField.FieldName).Append('(');
        var isEmpty = true;

        var selectValue = expandedField.SelectFields.BuildODataParameterValue();
        if (string.IsNullOrEmpty(selectValue) is false)
        {
            valueBuilder = valueBuilder.Append("$select=").Append(selectValue);
            isEmpty = false;
        }

        var expandValue = expandedField.ExpandFields.Map(BuildExpandFieldValue).BuildODataParameterValue();
        if (string.IsNullOrEmpty(expandValue) is false)
        {
            if (isEmpty is false)
            {
                valueBuilder = valueBuilder.Append(';');
            }

            valueBuilder = valueBuilder.Append("$expand=").Append(expandValue);
        }

        return valueBuilder.Append(')').ToString();
    }

    internal static string BuildQueryString(this IReadOnlyCollection<KeyValuePair<string, string>> queryParams)
    {
        if (queryParams.Count is not > 0)
        {
            return string.Empty;
        }

        var queryStringBuilder = new StringBuilder();

        foreach(var queryParam in queryParams.Where(NotEmptyValue))
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

        static bool NotEmptyValue(KeyValuePair<string, string> pair)
            =>
            string.IsNullOrEmpty(pair.Value) is false;
    }
}
