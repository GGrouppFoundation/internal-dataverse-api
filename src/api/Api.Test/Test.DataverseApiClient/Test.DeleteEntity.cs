using System;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Xunit;

namespace GGroupp.Infra.Dataverse.Api.Test;

partial class DataverseApiClientTest
{
    [Fact]
    public static  async Task DeleteEntityAsync_InputIsNull_ExpectArgumentNullException()
    {
        var mockHttpApi = CreateMockHttpApi<Unit, Unit>(default(Unit));
        var dataverseApiClient = CreateDataverseApiClient(mockHttpApi.Object);

        var token = new CancellationToken(canceled: false);
        var ex = await Assert.ThrowsAsync<ArgumentNullException>(InnerDeleteEntityAsync);

        Assert.Equal("input", ex.ParamName);

        Task InnerDeleteEntityAsync()
            =>
            dataverseApiClient.DeleteEntityAsync(null!, token).AsTask();
    }

    [Fact]
    public static  void DeleteEntityAsync_CancellationTokenIsCanceled_ExpectTaskIsCanceled()
    {
        var mockHttpApi = CreateMockHttpApi<Unit, Unit>(default(Unit));
        var dataverseApiClient = CreateDataverseApiClient(mockHttpApi.Object);

        var token = new CancellationToken(canceled: true);

        var actualTask = dataverseApiClient.DeleteEntityAsync(SomeDataverseEntityDeleteInput, token);
        Assert.True(actualTask.IsCanceled);
    }

    [Theory]
    [MemberData(nameof(ApiClientTestDataSource.GetEntityDeleteInputTestData), MemberType = typeof(ApiClientTestDataSource))]
    internal static  async Task DeleteEntityAsync_CancellationTokenIsNotCanceled_ExpectHttpRequestCalledOnce(
        Guid? callerId, DataverseEntityDeleteIn input, DataverseHttpRequest<Unit> expectedRequest)
    {
        var mockHttpApi = CreateMockHttpApi<StubRequestJson, StubResponseJson>(SomeResponseJson);
        var dataverseApiClient = CreateDataverseApiClient(mockHttpApi.Object, callerId);

        var token = new CancellationToken(canceled: false);
        _ = await dataverseApiClient.DeleteEntityAsync(input, token);

        mockHttpApi.Verify(p => p.InvokeAsync<Unit, Unit>(expectedRequest, token), Times.Once);
    }
    
    [Theory]
    [MemberData(nameof(ApiClientTestDataSource.GetFailureOutputTestData), MemberType = typeof(ApiClientTestDataSource))]
    public static  async Task DeleteEntityAsync_ResponseIsFailure_ExpectFailure(
        Failure<DataverseFailureCode> failure)
    {
        var mockHttpApi = CreateMockHttpApi<Unit, Unit>(failure);
        var dataverseApiClient = CreateDataverseApiClient(mockHttpApi.Object);

        var actual = await dataverseApiClient.DeleteEntityAsync(SomeDataverseEntityDeleteInput, default);
        Assert.Equal(failure, actual);
    }

    [Fact]
    public static  async Task DeleteEntityAsync_ResponseIsSuccess_ExpectSuccess()
    {
        var mockHttpApi = CreateMockHttpApi<Unit, Unit>(default(Unit));
        var dataverseApiClient = CreateDataverseApiClient(mockHttpApi.Object);

        var actual = await dataverseApiClient.DeleteEntityAsync(SomeDataverseEntityDeleteInput, default);
        var expected = Result.Success<Unit>(default);

        Assert.Equal(expected, actual);
    }
}