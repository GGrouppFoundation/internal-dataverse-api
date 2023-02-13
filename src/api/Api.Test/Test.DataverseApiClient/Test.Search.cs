using System;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Xunit;

namespace GGroupp.Infra.Dataverse.Api.Test;

partial class DataverseApiClientTest
{
    [Fact]
    public static  async Task SearchAsync_InputIsNull_ExpectArgumentNullException()
    {
        var mockHttpApi = CreateMockHttpApi<DataverseSearchJsonIn, DataverseSearchJsonOut>(SomeSearchJsonOut);
        var dataverseApiClient = CreateDataverseApiClient(mockHttpApi.Object);

        var token = new CancellationToken(canceled: false);
        var ex = await Assert.ThrowsAsync<ArgumentNullException>(InnerSearchAsync);

        Assert.Equal("input", ex.ParamName);

        Task InnerSearchAsync()
            =>
            dataverseApiClient.SearchAsync(null!, token).AsTask();
    }

    [Fact]
    public static  void SearchAsync_CancellationTokenIsCanceled_ExpectTaskIsCanceled()
    {
        var mockHttpApi = CreateMockHttpApi<DataverseSearchJsonIn, DataverseSearchJsonOut>(SomeSearchJsonOut);
        var dataverseApiClient = CreateDataverseApiClient(mockHttpApi.Object);

        var token = new CancellationToken(canceled: true);

        var actualTask = dataverseApiClient.SearchAsync(SomeDataverseSearchInput, token);
        Assert.True(actualTask.IsCanceled);
    }

    [Theory]
    [MemberData(nameof(ApiClientTestDataSource.GetSearchInputTestData), MemberType = typeof(ApiClientTestDataSource))]
    internal static  async Task SearchAsync_CancellationTokenIsNotCanceled_ExpectHttpRequestCalledOnce(
        Guid? callerId, DataverseSearchIn input, DataverseHttpRequest<DataverseSearchJsonIn> expectedRequest)
    {
        var mockHttpApi = CreateMockHttpApi<DataverseSearchJsonIn, DataverseSearchJsonOut>(SomeSearchJsonOut);
        var dataverseApiClient = CreateDataverseApiClient(mockHttpApi.Object, callerId);

        var token = new CancellationToken(canceled: false);
        _ = await dataverseApiClient.SearchAsync(input, token);

        mockHttpApi.Verify(p => p.InvokeAsync<DataverseSearchJsonIn, DataverseSearchJsonOut>(expectedRequest, token), Times.Once);
    }

    [Theory]
    [MemberData(nameof(ApiClientTestDataSource.GetFailureOutputTestData), MemberType = typeof(ApiClientTestDataSource))]
    public static  async Task SearchAsync_ResponseIsFailure_ExpectFailure(
        Failure<DataverseFailureCode> failure)
    {
        var mockHttpApi = CreateMockHttpApi<DataverseSearchJsonIn, DataverseSearchJsonOut>(failure);
        var dataverseApiClient = CreateDataverseApiClient(mockHttpApi.Object);

        var actual = await dataverseApiClient.SearchAsync(SomeDataverseSearchInput, CancellationToken.None);
        Assert.Equal(failure, actual);
    }

    [Theory]
    [MemberData(nameof(ApiClientTestDataSource.GetSearchOutputTestData), MemberType = typeof(ApiClientTestDataSource))]
    internal static  async Task SearchAsync_ResponseIsSuccess_ExpectSuccess(
        DataverseSearchJsonOut success, DataverseSearchOut expected)
    {
        var mockHttpApi = CreateMockHttpApi<DataverseSearchJsonIn, DataverseSearchJsonOut>(success);
        var dataverseApiClient = CreateDataverseApiClient(mockHttpApi.Object);

        var actual = await dataverseApiClient.SearchAsync(SomeDataverseSearchInput, default);
        Assert.Equal(expected, actual);
    }
}