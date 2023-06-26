using System;
using Xunit;

namespace GarageGroup.Infra.Dataverse.Api.Core.EntityKey.Tests;

partial class DataversePrimaryKeyTest
{
    [Theory]
    [InlineData("73bfb34f-ff97-47b8-af7b-1720168c1ef0")]
    [InlineData("00000000-0000-0000-0000-000000000000")]
    public void Constructor_ExpectValueIsEqualToSourceIdToString(string stringId)
    {
        var entityId = Guid.Parse(stringId);
        var entityKey = new DataversePrimaryKey(entityId);

        var actual = entityKey.Value;
        Assert.Equal(expected: stringId, actual: actual);
    }
}