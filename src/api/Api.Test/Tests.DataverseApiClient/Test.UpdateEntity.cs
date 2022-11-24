using System;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Xunit;

namespace GGroupp.Infra.Dataverse.Api.Test;

partial class DataverseApiClientTest
{
    [Fact]
    public async Task UpdateEntityAsync_InputIsNull_ExpectArgumentNullException()
    {
        var mockHttpApi = CreateMockHttpApi<StubRequestJson, StubResponseJson?>(SomeResponseJson);
        var dataverseApiClient = CreateDataverseApiClient(mockHttpApi.Object);

        var token = new CancellationToken(canceled: false);
        var ex = await Assert.ThrowsAsync<ArgumentNullException>(InnerUpdateEntityAsync);

        Assert.Equal("input", ex.ParamName);

        Task InnerUpdateEntityAsync()
            =>
            dataverseApiClient.UpdateEntityAsync<StubRequestJson, StubResponseJson?>(null!, token).AsTask();
    }

    [Fact]
    public void UpdateEntityAsync_CancellationTokenIsCanceled_ExpectTaskIsCanceled()
    {
        var mockHttpApi = CreateMockHttpApi<StubRequestJson, Unit>(default(Unit));
        var dataverseApiClient = CreateDataverseApiClient(mockHttpApi.Object);

        var input = SomeDataverseEntityUpdateInput;
        var token = new CancellationToken(canceled: true);

        var actualTask = dataverseApiClient.UpdateEntityAsync<StubRequestJson, Unit>(input, token);
        Assert.True(actualTask.IsCanceled);
    }

    [Theory]
    [MemberData(nameof(ApiClientTestDataSource.GetEntityUpdateInputTestData), MemberType = typeof(ApiClientTestDataSource))]
    internal async Task UpdateEntityAsync_CancellationTokenIsNotCanceled_ExpectHttpRequestCalledOnce(
        Guid? callerId, DataverseEntityUpdateIn<StubRequestJson> input, DataverseHttpRequest<StubRequestJson> expectedRequest)
    {
        var mockHttpApi = CreateMockHttpApi<StubRequestJson, StubResponseJson>(SomeResponseJson);
        var dataverseApiClient = CreateDataverseApiClient(mockHttpApi.Object, callerId);

        var token = new CancellationToken(canceled: false);
        _ = await dataverseApiClient.UpdateEntityAsync<StubRequestJson, StubResponseJson>(input, token);

        mockHttpApi.Verify(p => p.InvokeAsync<StubRequestJson, StubResponseJson>(expectedRequest, token), Times.Once);
    }

    [Theory]
    [MemberData(nameof(ApiClientTestDataSource.GetFailureOutputTestData), MemberType = typeof(ApiClientTestDataSource))]
    public async Task UpdateEntityAsync_ResponseIsFailure_ExpectFailure(
        Failure<DataverseFailureCode> failure)
    {
        var mockHttpApi = CreateMockHttpApi<StubRequestJson, StubResponseJson>(failure);
        var dataverseApiClient = CreateDataverseApiClient(mockHttpApi.Object);

        var input = SomeDataverseEntityUpdateInput;
        var actual = await dataverseApiClient.UpdateEntityAsync<StubRequestJson, StubResponseJson>(input, default);

        Assert.Equal(failure, actual);
    }

    [Theory]
    [MemberData(nameof(ApiClientTestDataSource.GetStubResponseJsonOutputTestData), MemberType = typeof(ApiClientTestDataSource))]
    internal async Task UpdateEntityAsync_ResponseIsSuccess_ExpectSuccess(
        StubResponseJson? success)
    {
        var mockHttpApi = CreateMockHttpApi<StubRequestJson, StubResponseJson?>(success);
        var dataverseApiClient = CreateDataverseApiClient(mockHttpApi.Object);

        var input = SomeDataverseEntityUpdateInput;
        var actual = await dataverseApiClient.UpdateEntityAsync<StubRequestJson, StubResponseJson>(input, default);

        var expected = new DataverseEntityUpdateOut<StubResponseJson>(success);
        Assert.Equal(expected, actual);
    }
}