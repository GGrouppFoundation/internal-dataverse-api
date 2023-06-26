using System;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Xunit;

namespace GarageGroup.Infra.Dataverse.Api.Test;

partial class DataverseApiClientTest
{
    [Fact]
    public static async Task UpdateEntityAsync_InputIsNull_ExpectArgumentNullException()
    {
        var mockHttpApi = CreateMockHttpApi<StubRequestJson, Unit>(default(Unit));
        var dataverseApiClient = CreateDataverseApiClient(mockHttpApi.Object);

        var token = new CancellationToken(canceled: false);
        var ex = await Assert.ThrowsAsync<ArgumentNullException>(InnerUpdateEntityAsync);

        Assert.Equal("input", ex.ParamName);

        Task InnerUpdateEntityAsync()
            =>
            dataverseApiClient.UpdateEntityAsync<StubRequestJson>(null!, token).AsTask();
    }

    [Fact]
    public static void UpdateEntityAsync_CancellationTokenIsCanceled_ExpectTaskIsCanceled()
    {
        var mockHttpApi = CreateMockHttpApi<StubRequestJson, Unit>(default(Unit));
        var dataverseApiClient = CreateDataverseApiClient(mockHttpApi.Object);

        var input = SomeDataverseEntityUpdateInput;
        var cancellationToken = new CancellationToken(canceled: true);

        var actualTask = dataverseApiClient.UpdateEntityAsync(input, cancellationToken);
        Assert.True(actualTask.IsCanceled);
    }

    [Theory]
    [MemberData(nameof(ApiClientTestDataSource.EntityUpdateInputTestData), MemberType = typeof(ApiClientTestDataSource))]
    internal static async Task UpdateEntityAsync_CancellationTokenIsNotCanceled_ExpectHttpRequestCalledOnce(
        Guid? callerId, DataverseEntityUpdateIn<StubRequestJson> input, DataverseHttpRequest<StubRequestJson> expectedRequest)
    {
        var mockHttpApi = CreateMockHttpApi<StubRequestJson, Unit>(default(Unit));
        var dataverseApiClient = CreateDataverseApiClient(mockHttpApi.Object, callerId);

        var token = new CancellationToken(canceled: false);
        _ = await dataverseApiClient.UpdateEntityAsync(input, token);

        mockHttpApi.Verify(p => p.InvokeAsync<StubRequestJson, Unit>(expectedRequest, token), Times.Once);
    }

    [Theory]
    [MemberData(nameof(ApiClientTestDataSource.FailureOutputTestData), MemberType = typeof(ApiClientTestDataSource))]
    public static async Task UpdateEntityAsync_ResponseIsFailure_ExpectFailure(
        Failure<DataverseFailureCode> failure)
    {
        var mockHttpApi = CreateMockHttpApi<StubRequestJson, Unit>(failure);
        var dataverseApiClient = CreateDataverseApiClient(mockHttpApi.Object);

        var input = SomeDataverseEntityUpdateInput;
        var actual = await dataverseApiClient.UpdateEntityAsync(input, default);

        Assert.StrictEqual(failure, actual);
    }

    [Fact]
    public static async Task UpdateEntityAsync_ResponseIsSuccess_ExpectSuccess()
    {
        var mockHttpApi = CreateMockHttpApi<StubRequestJson, Unit>(default(Unit));
        var dataverseApiClient = CreateDataverseApiClient(mockHttpApi.Object);

        var actual = await dataverseApiClient.UpdateEntityAsync(SomeDataverseEntityUpdateInput, default);
        var expected = default(Unit);

        Assert.StrictEqual(expected, actual);
    }
}