using Xunit;

namespace GarageGroup.Infra.Dataverse.Api.Test;

partial class ApiClientTestDataSource
{
    public static TheoryData<StubResponseJson?> StubResponseJsonOutputTestData
        =>
        new()
        {
            {
                default
            },
            {
                new()
                {
                    Id = 15,
                    Name = "Some name"
                }
            }
        };
}