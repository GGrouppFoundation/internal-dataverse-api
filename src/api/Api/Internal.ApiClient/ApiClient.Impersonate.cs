using System;

namespace GGroupp.Infra;

partial class DataverseApiClient
{
    public IDataverseApiClient Impersonate(Guid callerId)
        =>
        new DataverseApiClient(httpApi, callerId);
}