using Xunit;

namespace GGroupp.Infra.Dataverse.Api.Core.EntityKey.Tests;

partial class DataverseAlternateKeyTest
{
    [Theory]
    [InlineData(null, null, "%3D")]
    [InlineData(null, "", "%3D")]
    [InlineData("", null, "%3D")]
    [InlineData("", "", "%3D")]
    [InlineData("a", "b", "a%3Db")]
    [InlineData("ac", "", "ac%3D")]
    [InlineData("", "a", "%3Da")]
    [InlineData("a=d", "b ", "a%3Dd%3Db+")]
    [InlineData("a/b", @"a\b", "a%2Fb%3Da%5Cb")]
    public void ConstructorSingle_ArgumentsAreSingle_ExpectCorrectValue(
        string field, string value, string expectedValue)
    {
        var actual = new DataverseAlternateKey(field, value);
        var actualValue = actual.Value;

        Assert.Equal(expectedValue, actualValue);
    }
}