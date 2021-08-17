#nullable enable

using System.Collections.Generic;
using Xunit;

namespace GGroupp.Infra.Dataverse.Api.Core.EntityKey.Tests
{
    partial class DataverseAlternateKeyTest
    {
        [Fact]
        public void Constructor_AnyId_TryingEmptyDictioanryArguments()
        {
            var entityKey = new DataverseAlternateKey(
                idArguments: new Dictionary<string, string>());
            Assert.Empty(entityKey.Value);
        }

        [Fact]
        public void Constructor_AnyId_TryingNullArguments()
        {
            var entityKey = new DataverseAlternateKey(idArguments: null!);
            Assert.Empty(entityKey.Value);
        }

        [Fact]
        public void Constructor_AnyId_TryingOneElementArguments()
        {
            var input = new Dictionary<string, string>()
            {
                { "a", "b" }
            };
            var entityKey = new DataverseAlternateKey(idArguments: input);
            Assert.Equal(expected: "a%3Db", actual: entityKey.Value);
        }

        [Fact]
        public void Constructor_AnyId_TryingSeveralElementArguments()
        {
            var input = new Dictionary<string, string>()
            {
                { "a", "b" }, { "ac", "" }, { "ad", "b" }, { "ab", "ab" },
            };
            var entityKey = new DataverseAlternateKey(idArguments: input);
            Assert.Equal(expected: "a%3Db,ad%3Db,ab%3Dab", actual: entityKey.Value);
        }

        [Fact]
        public void Constructor_AnyId_TryingEscapeElemntsInArguments()
        {
            var input = new Dictionary<string, string>()
            {
                { "a", "b" }, { "a-c", "" }, { "a=d", "b " }, { "a/b", @"a\b" },
            };
            var entityKey = new DataverseAlternateKey(idArguments: input);
            Assert.Equal(expected: @"a%3Db,a%3Dd%3Db+,a%2Fb%3Da%5Cb", actual: entityKey.Value);
        }

        [Fact]
        public void Constructor_AnyId_TryingSeveralUnacceptableElementArguments()
        {
            var input = new Dictionary<string, string>()
            {
                { "a", "" }, { "ac", "" }, { "ad", "" }, { "ab", "" },
            };
            var entityKey = new DataverseAlternateKey(idArguments: input);
            Assert.Empty(entityKey.Value);
        }
    }
}
