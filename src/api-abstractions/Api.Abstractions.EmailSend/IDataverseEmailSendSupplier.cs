using System;
using System.Threading;
using System.Threading.Tasks;

namespace GGroupp.Infra;

public interface IDataverseEmailSendSupplier
{
    ValueTask<Result<DataverseEmailCreateOut, Failure<DataverseFailureCode>>> CreateEmailAsync(
       DataverseEmailCreateIn input, CancellationToken cancellationToken = default);
    
    ValueTask<Result<DataverseEmailSendOut, Failure<DataverseFailureCode>>> SendEmailAsync(
        DataverseEmailSendIn input, CancellationToken cancellationToken = default);
}