using Xunit;

namespace GarageGroup.Infra.Dataverse.Api.Core.EntityKey.Test;

partial class DataverseAlternateKeyTest
{
    [Theory]
    [InlineData(null, null, "=")]
    [InlineData(null, "", "=")]
    [InlineData("", null, "=")]
    [InlineData("", "", "=")]
    [InlineData("a", "b", "a=b")]
    [InlineData("ac", "", "ac=")]
    [InlineData("", "a", "=a")]
    [InlineData("a=d", "b ", "a=d=b ")]
    [InlineData("a/b", @"a\b", @"a/b=a\b")]
    public void ConstructorSingle_ArgumentsAreSingle_ExpectCorrectValue(
        string? field, string? value, string expectedValue)
    {
        var actual = new DataverseAlternateKey(field!, value!);
        var actualValue = actual.Value;

        Assert.Equal(expectedValue, actualValue);
    }
}