using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GGroupp.Infra;

partial class DataverseApiClient
{
    public ValueTask<Result<DataverseEntitySetGetOut<TJson>, Failure<DataverseFailureCode>>> GetEntitySetAsync<TJson>(
        DataverseEntitySetGetIn input, CancellationToken cancellationToken = default)
    {
        _ = input ?? throw new ArgumentNullException(nameof(input));

        if (cancellationToken.IsCancellationRequested)
        {
            return GetCanceledAsync<DataverseEntitySetGetOut<TJson>>(cancellationToken);
        }

        return InnerGetEntitySetAsync<TJson>(input, cancellationToken);
    }

    private async ValueTask<Result<DataverseEntitySetGetOut<TJson>, Failure<DataverseFailureCode>>> InnerGetEntitySetAsync<TJson>(
        DataverseEntitySetGetIn input, CancellationToken cancellationToken)
    {
        using var httpClient = CreateDataHttpClient();
        var entitiesGetUrl = BuildEntitySetGetUrl(input);

        var response = await httpClient.GetAsync(entitiesGetUrl, cancellationToken).ConfigureAwait(false);
        var result = await response.InternalReadDataverseResultAsync<DataverseEntitySetJsonGetOut<TJson>>(cancellationToken);

        return result.MapSuccess(e => new DataverseEntitySetGetOut<TJson>(e?.Value));
    }
    
    private static string BuildEntitySetGetUrl(DataverseEntitySetGetIn input)
        =>
        Pipeline.Pipe(
            new Dictionary<string, string>
            { 
                ["$select"] = QueryParametersBuilder.InternalBuildOdataParameterValue(input.SelectFields),
                ["$filter"] = input.Filter,
                ["$orderby"] = string.Join(',', input.OrderBy.Where(NotEmptyFieldName).Select(GetOrderByValue))
            })
        .Pipe(
            parameters =>
            {
                if (input.Top.HasValue)
                {
                    parameters.Add("$top", input.Top.Value.ToString(CultureInfo.InvariantCulture));
                }
                return (IReadOnlyCollection<KeyValuePair<string, string>>)parameters;
            })
        .Pipe(
            QueryParametersBuilder.InternalBuildQueryString)
        .Pipe(
            queryString => input.EntityPluralName + queryString);

    private static bool NotEmptyFieldName(DataverseOrderParameter orderParameter)
        =>
        string.IsNullOrEmpty(orderParameter.FieldName) is false;

    private static string GetOrderByValue(DataverseOrderParameter orderParameter)
        =>
        orderParameter.Direction switch
        {
            DataverseOrderDirection.Ascending => $"{orderParameter.FieldName} asc",
            DataverseOrderDirection.Descending => $"{orderParameter.FieldName} desc",
            _ => orderParameter.FieldName
        };
}