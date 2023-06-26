using System;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Xunit;

namespace GarageGroup.Infra.Dataverse.Api.Test;

partial class DataverseApiClientTest
{
    [Fact]
    public static async Task CreateEntityAsync_InputIsNull_ExpectArgumentNullException()
    {
        var mockHttpApi = CreateMockHttpApi<StubRequestJson, Unit>(default(Unit));
        var dataverseApiClient = CreateDataverseApiClient(mockHttpApi.Object);

        var token = new CancellationToken(canceled: false);
        var ex = await Assert.ThrowsAsync<ArgumentNullException>(InnerCreateEntityAsync);

        Assert.Equal("input", ex.ParamName);

        Task InnerCreateEntityAsync()
            =>
            dataverseApiClient.CreateEntityAsync<StubRequestJson>(null!, token).AsTask();
    }

    [Fact]
    public static void CreateEntityAsync_CancellationTokenIsCanceled_ExpectTaskIsCanceled()
    {
        var mockHttpApi = CreateMockHttpApi<StubRequestJson, Unit>(default(Unit));
        var dataverseApiClient = CreateDataverseApiClient(mockHttpApi.Object);

        var token = new CancellationToken(canceled: true);
        var actualTask = dataverseApiClient.CreateEntityAsync(SomeDataverseEntityCreateInput, token);

        Assert.True(actualTask.IsCanceled);
    }

    [Theory]
    [MemberData(nameof(ApiClientTestDataSource.EntityCreateInputTestData), MemberType = typeof(ApiClientTestDataSource))]
    internal static async Task CreateEntityAsync_CancellationTokenIsNotCanceled_ExpectHttpRequestCalledOnce(
        Guid? callerId, DataverseEntityCreateIn<StubRequestJson> input, DataverseHttpRequest<StubRequestJson> expectedRequest)
    {
        var mockHttpApi = CreateMockHttpApi<StubRequestJson, Unit>(default(Unit));
        var dataverseApiClient = CreateDataverseApiClient(mockHttpApi.Object, callerId);

        var token = new CancellationToken(canceled: false);
        _ = await dataverseApiClient.CreateEntityAsync(input, token);

        mockHttpApi.Verify(p => p.InvokeAsync<StubRequestJson, Unit>(expectedRequest, token), Times.Once);
    }

    [Theory]
    [MemberData(nameof(ApiClientTestDataSource.FailureOutputTestData), MemberType = typeof(ApiClientTestDataSource))]
    public static async Task CreateEntityAsync_ResponseIsFailure_ExpectFailure(
        Failure<DataverseFailureCode> failure)
    {
        var mockHttpApi = CreateMockHttpApi<StubRequestJson, Unit>(failure);
        var dataverseApiClient = CreateDataverseApiClient(mockHttpApi.Object);

        var input = SomeDataverseEntityCreateInput;
        var actual = await dataverseApiClient.CreateEntityAsync(input, default);

        Assert.StrictEqual(failure, actual);
    }

    [Fact]
    public static async Task CreateEntityAsync_ResponseIsSuccess_ExpectSuccess()
    {
        var mockHttpApi = CreateMockHttpApi<StubRequestJson, Unit>(default(Unit));
        var dataverseApiClient = CreateDataverseApiClient(mockHttpApi.Object);

        var actual = await dataverseApiClient.CreateEntityAsync(SomeDataverseEntityCreateInput, default);
        var expected = default(Unit);

        Assert.StrictEqual(expected, actual);
    }
}