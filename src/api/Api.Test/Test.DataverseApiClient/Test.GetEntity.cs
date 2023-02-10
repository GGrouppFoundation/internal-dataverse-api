using System;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Xunit;

namespace GGroupp.Infra.Dataverse.Api.Test;

partial class DataverseApiClientTest
{
    [Fact]
    public static async Task GetEntityAsync_InputIsNull_ExpectArgumentNullException()
    {
        var mockHttpApi = CreateMockHttpApi<Unit, StubResponseJson>(SomeResponseJson);
        var dataverseApiClient = CreateDataverseApiClient(mockHttpApi.Object);

        var token = new CancellationToken(canceled: false);
        var ex = await Assert.ThrowsAsync<ArgumentNullException>(InnerGetEntityAsync);

        Assert.Equal("input", ex.ParamName);

        Task InnerGetEntityAsync()
            =>
            dataverseApiClient.GetEntityAsync<StubResponseJson>(null!, token).AsTask();
    }

    [Fact]
    public static void GetEntityAsync_CancellationTokenIsCanceled_ExpectTaskIsCanceled()
    {
        var mockHttpApi = CreateMockHttpApi<Unit, StubResponseJson>(SomeResponseJson);
        var dataverseApiClient = CreateDataverseApiClient(mockHttpApi.Object);

        var token = new CancellationToken(canceled: true);

        var actualTask = dataverseApiClient.GetEntityAsync<StubResponseJson>(SomeDataverseEntityGetInput, token);
        Assert.True(actualTask.IsCanceled);
    }

    [Theory]
    [MemberData(nameof(ApiClientTestDataSource.EntityGetInputTestData), MemberType = typeof(ApiClientTestDataSource))]
    internal static async Task GetEntityAsync_CancellationTokenIsNotCanceled_ExpectHttpRequestCalledOnce(
        Guid? callerId, DataverseEntityGetIn input, DataverseHttpRequest<Unit> expectedRequest)
    {
        var mockHttpApi = CreateMockHttpApi<Unit, StubResponseJson>(SomeResponseJson);
        var dataverseApiClient = CreateDataverseApiClient(mockHttpApi.Object, callerId);

        var token = new CancellationToken(canceled: false);
        _ = await dataverseApiClient.GetEntityAsync<StubResponseJson>(input, token);

        mockHttpApi.Verify(p => p.InvokeAsync<Unit, StubResponseJson>(expectedRequest, token), Times.Once);
    }

    [Theory]
    [MemberData(nameof(ApiClientTestDataSource.FailureOutputTestData), MemberType = typeof(ApiClientTestDataSource))]
    public static async Task GetEntityAsync_ResponseIsFailure_ExpectFailure(
        Failure<DataverseFailureCode> failure)
    {
        var mockHttpApi = CreateMockHttpApi<Unit, StubResponseJson>(failure);
        var dataverseApiClient = CreateDataverseApiClient(mockHttpApi.Object);

        var actual = await dataverseApiClient.GetEntityAsync<StubResponseJson>(SomeDataverseEntityGetInput, default);
        Assert.StrictEqual(failure, actual);
    }

    [Theory]
    [MemberData(nameof(ApiClientTestDataSource.StubResponseJsonOutputTestData), MemberType = typeof(ApiClientTestDataSource))]
    internal static async Task GetEntityAsync_ResponseIsSuccess_ExpectSuccess(
        StubResponseJson? success)
    {
        var mockHttpApi = CreateMockHttpApi<Unit, StubResponseJson>(success);
        var dataverseApiClient = CreateDataverseApiClient(mockHttpApi.Object);

        var actual = await dataverseApiClient.GetEntityAsync<StubResponseJson>(SomeDataverseEntityGetInput, default);
        var expected = new DataverseEntityGetOut<StubResponseJson>(success);

        Assert.StrictEqual(expected, actual);
    }
}