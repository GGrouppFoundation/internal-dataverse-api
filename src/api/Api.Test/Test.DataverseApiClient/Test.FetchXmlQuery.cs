using System;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using Xunit;

namespace GarageGroup.Infra.Dataverse.Api.Test;

partial class DataverseApiClientTest
{
    [Fact]
    public static async Task FetchXmlAsync_InputIsNull_ExpectArgumentNullException()
    {
        // Arrange
        var mockHttpApi = CreateMockHttpApi<Unit, DataverseEntitySetJsonGetOut<StubResponseJson>>(SomeResponseJsonSet);
        var dataverseApiClient = CreateDataverseApiClient(mockHttpApi.Object);

        var token = new CancellationToken(canceled: false);

        // Act
        var ex = await Assert.ThrowsAsync<ArgumentNullException>(InnerGetEntitySetAsync);

        // Assert
        Assert.Equal("input", ex.ParamName);

        Task InnerGetEntitySetAsync()
            =>
            dataverseApiClient.FetchXmlAsync<StubResponseJson>(null!, token).AsTask();
    }

    [Fact]
    public static void FetchXmlAsync_CancellationTokenIsCanceled_ExpectTaskIsCanceled()
    {
        // Arrange
        var mockHttpApi = CreateMockHttpApi<Unit, DataverseEntitySetJsonGetOut<StubResponseJson>>(SomeResponseJsonSet);
        var dataverseApiClient = CreateDataverseApiClient(mockHttpApi.Object);

        var token = new CancellationToken(canceled: true);
        
        // Act
        var actualTask = dataverseApiClient.FetchXmlAsync<StubResponseJson>(new Fixture().Create<DataverseFetchXmlIn>(), token);
        
        // Assert
        Assert.True(actualTask.IsCanceled);
    }
    
    [Theory]
    [MemberData(nameof(ApiClientTestDataSource.GetFetchXmlInputTestData), MemberType = typeof(ApiClientTestDataSource))]
    internal static async Task FetchXmlAsync_CancellationTokenIsNotCanceled_ExpectHttpRequestCalledOnce(
        DataverseFetchXmlIn input, DataverseHttpRequest<Unit> expectedRequest)
    {
        // Arrange
        var fixture = new Fixture();
        var unCustomizedFixture = new Fixture();
        
        var customization = new SupportMutableValueTypesCustomization();
        customization.Customize(fixture);
        fixture.Register<string>(() => $"<cookie pagenumber='2' pagingcookie='{unCustomizedFixture.Create<string>()}'/>");
        
        var mockHttpApi = CreateMockHttpApi<Unit, DataverseFetchXmlOutJson<StubResponseJson>>(
            fixture.Create<Result<DataverseFetchXmlOutJson<StubResponseJson>,Failure<DataverseFailureCode>>>());
        var dataverseApiClient = CreateDataverseApiClient(mockHttpApi.Object);

        var token = new CancellationToken(canceled: false);
        
        // Act
        _ = await dataverseApiClient.FetchXmlAsync<StubResponseJson>(input, token);

        // Assert        
        mockHttpApi.Verify(p => p.InvokeAsync<Unit, DataverseFetchXmlOutJson<StubResponseJson>>(expectedRequest, token), Times.Once);
    }
    
    [Theory]
    [MemberData(nameof(ApiClientTestDataSource.FailureOutputTestData), MemberType = typeof(ApiClientTestDataSource))]
    public static async Task FetchXmlAsync_ResponseIsFailure_ExpectFailure(
        Failure<DataverseFailureCode> failure)
    {
        // Arrange
        var mockHttpApi = CreateMockHttpApi<Unit, DataverseFetchXmlOutJson<StubResponseJson>>(failure);
        var dataverseApiClient = CreateDataverseApiClient(mockHttpApi.Object);
        
        var input = new Fixture().Create<DataverseFetchXmlIn>();
        
        // Act
        var actual = await dataverseApiClient.FetchXmlAsync<StubResponseJson>(input, CancellationToken.None);

        // Assert
        Assert.StrictEqual(failure, actual);
    }
    
    [Theory]
    [MemberData(nameof(ApiClientTestDataSource.FetchStubResponseJsonTestData), MemberType = typeof(ApiClientTestDataSource))]
    internal static async Task FetchXmlAsync_ResponseIsSuccess_ExpectSuccess(
        DataverseFetchXmlOutJson<StubResponseJson> success, DataverseFetchXmlOut<StubResponseJson> expected)
    {
        // Arrange
        var mockHttpApi = CreateMockHttpApi<Unit, DataverseFetchXmlOutJson<StubResponseJson>>(success);
        var dataverseApiClient = CreateDataverseApiClient(mockHttpApi.Object);
        var input = new Fixture().Create<DataverseFetchXmlIn>();

        // Act
        var actual = await dataverseApiClient.FetchXmlAsync<StubResponseJson>(input, CancellationToken.None);
        
        // Assert
        Assert.StrictEqual(expected, actual);
    }
}