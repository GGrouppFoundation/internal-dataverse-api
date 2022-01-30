using System.Collections.Generic;
using Xunit;

namespace GGroupp.Infra.Dataverse.Api.Core.EntityKey.Tests;

partial class DataverseAlternateKeyTest
{
    [Fact]
    public void Constructor_ArgumentsAreNull_ExpectEmpty()
    {
        var entityKey = new DataverseAlternateKey(idArguments: null!);
        Assert.Empty(entityKey.Value);
    }

    [Fact]
    public void Constructor_ArgumentsAreEmpty_ExpectEmpty()
    {
        var entityKey = new DataverseAlternateKey(
            idArguments: new Dictionary<string, string>());
        Assert.Empty(entityKey.Value);
    }

    [Fact]
    public void Constructor_ArgumentsAreSingle_ExpectCorrectValue()
    {
        var input = new Dictionary<string, string>()
        {
            { "a", "b" }
        };

        var entityKey = new DataverseAlternateKey(idArguments: input);
        Assert.Equal(expected: "a%3Db", actual: entityKey.Value);
    }

    [Fact]
    public void Constructor_ArgumentsAreSeveral_ExpectCorrectValue()
    {
        var input = new Dictionary<string, string>()
        {
            { "a", "b" }, { "ac", "" }, { "ad", "b" }, { "ab", "ab" },
        };

        var entityKey = new DataverseAlternateKey(idArguments: input);
        Assert.Equal(expected: "a%3Db,ad%3Db,ab%3Dab", actual: entityKey.Value);
    }

    [Fact]
    public void Constructor_ArgumentsAreSeveral_ExpectCorrectEscapedValue()
    {
        var input = new Dictionary<string, string>()
        {
            { "a", "b" }, { "a-c", string.Empty }, { "a=d", "b " }, { "a/b", @"a\b" },
        };

        var entityKey = new DataverseAlternateKey(idArguments: input);
        Assert.Equal(expected: @"a%3Db,a%3Dd%3Db+,a%2Fb%3Da%5Cb", actual: entityKey.Value);
    }

    [Fact]
    public void Constructor_ArgumentValuesAreNullOrEmpty_ExpectEmpty()
    {
        var input = new Dictionary<string, string>()
        {
            { "a", string.Empty }, { "ac", null! }, { "ad", string.Empty }, { "ab", string.Empty },
        };

        var entityKey = new DataverseAlternateKey(idArguments: input);
        Assert.Empty(entityKey.Value);
    }
}