using System;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Xunit;

namespace GGroupp.Infra.Dataverse.Api.Test;

partial class DataverseApiClientTest
{
    [Fact]
    public static  async Task GetEntitySetAsync_InputIsNull_ExpectArgumentNullException()
    {
        var mockHttpApi = CreateMockHttpApi<Unit, DataverseEntitySetJsonGetOut<StubResponseJson>>(SomeResponseJsonSet);
        var dataverseApiClient = CreateDataverseApiClient(mockHttpApi.Object);

        var token = new CancellationToken(canceled: false);

        var ex = await Assert.ThrowsAsync<ArgumentNullException>(InnerGetEntitySetAsync);

        Assert.Equal("input", ex.ParamName);

        Task InnerGetEntitySetAsync()
            =>
            dataverseApiClient.GetEntitySetAsync<StubResponseJson>(null!, token).AsTask();
    }

    [Fact]
    public static  void GetEntitySetAsync_CancellationTokenIsCanceled_ExpectTaskIsCanceled()
    {
        var mockHttpApi = CreateMockHttpApi<Unit, DataverseEntitySetJsonGetOut<StubResponseJson>>(SomeResponseJsonSet);
        var dataverseApiClient = CreateDataverseApiClient(mockHttpApi.Object);

        var input = SomeDataverseEntitySetGetInput;
        var token = new CancellationToken(canceled: true);

        var actualTask = dataverseApiClient.GetEntitySetAsync<StubResponseJson>(input, token);
        Assert.True(actualTask.IsCanceled);
    }

    [Theory]
    [MemberData(nameof(ApiClientTestDataSource.GetEntitySetGetInputTestData), MemberType = typeof(ApiClientTestDataSource))]
    internal static  async Task GetEntitySetAsync_CancellationTokenIsNotCanceled_ExpectHttpRequestCalledOnce(
        Guid? callerId, DataverseEntitySetGetIn input, DataverseHttpRequest<Unit> expectedRequest)
    {
        var mockHttpApi = CreateMockHttpApi<Unit, DataverseEntitySetJsonGetOut<StubResponseJson>>(SomeResponseJsonSet);
        var dataverseApiClient = CreateDataverseApiClient(mockHttpApi.Object, callerId);

        var token = new CancellationToken(canceled: false);
        _ = await dataverseApiClient.GetEntitySetAsync<StubResponseJson>(input, token);

        mockHttpApi.Verify(p => p.InvokeAsync<Unit, DataverseEntitySetJsonGetOut<StubResponseJson>>(expectedRequest, token), Times.Once);
    }

    [Theory]
    [MemberData(nameof(ApiClientTestDataSource.GetFailureOutputTestData), MemberType = typeof(ApiClientTestDataSource))]
    public static  async Task GetEntitySetAsync_ResponseIsFailure_ExpectFailure(
        Failure<DataverseFailureCode> failure)
    {
        var mockHttpApi = CreateMockHttpApi<Unit, DataverseEntitySetJsonGetOut<StubResponseJson>>(failure);
        var dataverseApiClient = CreateDataverseApiClient(mockHttpApi.Object);

        var input = SomeDataverseEntitySetGetInput;
        var actual = await dataverseApiClient.GetEntitySetAsync<StubResponseJson>(input, CancellationToken.None);

        Assert.Equal(failure, actual);
    }

    [Theory]
    [MemberData(nameof(ApiClientTestDataSource.GetStubResponseJsonSetOutputTestData), MemberType = typeof(ApiClientTestDataSource))]
    internal static  async Task GetEntitySetAsync_ResponseIsSuccess_ExpectSuccess(
        DataverseEntitySetJsonGetOut<StubResponseJson> success, DataverseEntitySetGetOut<StubResponseJson> expected)
    {
        var mockHttpApi = CreateMockHttpApi<Unit, DataverseEntitySetJsonGetOut<StubResponseJson>>(success);
        var dataverseApiClient = CreateDataverseApiClient(mockHttpApi.Object);

        var actual = await dataverseApiClient.GetEntitySetAsync<StubResponseJson>(SomeDataverseEntitySetGetInput, CancellationToken.None);
        Assert.Equal(expected, actual);
    }
}