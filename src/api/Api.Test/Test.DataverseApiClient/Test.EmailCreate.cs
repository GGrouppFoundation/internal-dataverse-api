using System;
using System.IO;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using Xunit;

namespace GGroupp.Infra.Dataverse.Api.Test;

partial class DataverseApiClientTest
{
    [Fact]
    internal static async Task CreateEmailAsync_InputIsNull_ExpectArgumentNullException()
    {
        // Arrange
        var mockHttpApi = CreateMockHttpApi<DataverseEmailCreateJsonIn, DataverseEmailCreateJsonOut>(SomeEmailCreateJsonOut);
        var dataverseApiClient = CreateDataverseApiClient(mockHttpApi.Object);

        var token = new CancellationToken(canceled: false);
        
        // Act
        var ex = await Assert.ThrowsAsync<ArgumentNullException>(InnerCreateEmailAsync);

        // Assert
        Assert.Equal("input", ex.ParamName);

        Task InnerCreateEmailAsync()
            =>
            dataverseApiClient.CreateEmailAsync(null!, token).AsTask();
    }

    [Fact]
    internal static void CreateEmailAsync_CancellationTokenIsCanceled_ExpectTaskIsCanceled()
    {
        // Arrange
        var mockHttpApi = CreateMockHttpApi<DataverseEmailCreateJsonIn, DataverseEmailCreateJsonOut>(SomeEmailCreateJsonOut);
        var dataverseApiClient = CreateDataverseApiClient(mockHttpApi.Object);

        var token = new CancellationToken(canceled: true);

        // Act
        var actualTask = dataverseApiClient.CreateEmailAsync(
            SomeEmailCreateIn, token);

        // Assert
        Assert.True(actualTask.IsCanceled);
    }
    
    [Theory]
    [MemberData(nameof(ApiClientTestDataSource.GetEmailCreateInputTestData), MemberType = typeof(ApiClientTestDataSource))]
    internal static async Task CreateEmailAsync_CancellationTokenIsNotCanceled_ExpectHttpRequestCalledOnce(
        DataverseEmailCreateIn input, DataverseHttpRequest<DataverseEmailCreateJsonIn> expectedRequest)
    {
        // Arrange
        var mockHttpApi = CreateMockHttpApi<DataverseEmailCreateJsonIn, DataverseEmailCreateJsonOut>(SomeEmailCreateJsonOut);
        var dataverseApiClient = CreateDataverseApiClient(mockHttpApi.Object);

        var token = new CancellationToken(canceled: false);
        
        // Act
        _ = await dataverseApiClient.CreateEmailAsync(input, token);

        // Assert
        mockHttpApi.Verify(p => p.InvokeAsync<DataverseEmailCreateJsonIn, DataverseEmailCreateJsonOut>(expectedRequest, token), Times.Once);
    }

    [Theory]
    [MemberData(nameof(ApiClientTestDataSource.GetInvalidEmailCreateInputTestData), MemberType = typeof(ApiClientTestDataSource))]
    internal static async Task CreateEmailAsync_PassNoSender_ExpectFailure(DataverseEmailCreateIn invalidInput)
    {
        // Arrange
        var mockHttpApi = CreateMockHttpApi<DataverseEmailCreateJsonIn, DataverseEmailCreateJsonOut>(SomeEmailCreateJsonOut);
        var dataverseApiClient = CreateDataverseApiClient(mockHttpApi.Object);
        
        var token = new CancellationToken(canceled: false);

        // Act
        var result = await dataverseApiClient.CreateEmailAsync(invalidInput, token);
        
        // Assert
        Assert.Equal(Failure.Create(DataverseFailureCode.Unknown, "Input is invalid"), result);
    }
    
    [Theory]
    [MemberData(nameof(ApiClientTestDataSource.GetFailureOutputTestData), MemberType = typeof(ApiClientTestDataSource))]
    public static  async Task CreateEmailAsync_ResponseIsFailure_ExpectFailure(
        Failure<DataverseFailureCode> failure)
    {
        // Arrange
        var mockHttpApi = CreateMockHttpApi<DataverseEmailCreateJsonIn, DataverseEmailCreateJsonOut>(failure);
        var dataverseApiClient = CreateDataverseApiClient(mockHttpApi.Object);
        

        // Act
        var actual = await dataverseApiClient.CreateEmailAsync(SomeEmailCreateIn, CancellationToken.None);
        
        // Assert
        Assert.Equal(failure, actual);
    }
}