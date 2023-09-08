using System;

namespace GarageGroup.Infra;

partial class DataverseApiClient
{
    public IDataverseApiClient Impersonate(Guid callerId)
        =>
        new DataverseApiClient(httpApi, guidProvider, callerId);
}