using System;
using System.Threading;
using System.Threading.Tasks;

namespace GGroupp.Infra;

public interface IDataverseFetchXmlSupplier
{
    ValueTask<Result<DataverseFetchXmlOut<TEntityJson>, Failure<DataverseFailureCode>>> FetchXmlAsync<TEntityJson>(
        DataverseFetchXmlIn input, CancellationToken cancellationToken = default);
}