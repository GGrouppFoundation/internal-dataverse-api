using System.Net.Http;
using Xunit;

namespace GarageGroup.Infra.Dataverse.Api.Test;

partial class HttpApiTestDataSource
{
    public static TheoryData<StringContent?> UnitJsonSuccessTestData
        =>
        new()
        {
            {
                new("Some string")
            },
            {
                new(string.Empty)
            },
            {
                null
            }
        };
}