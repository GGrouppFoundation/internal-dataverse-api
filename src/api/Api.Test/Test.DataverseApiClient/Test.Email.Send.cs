using System;
using System.Threading;
using System.Threading.Tasks;
using DeepEqual.Syntax;
using Moq;
using Xunit;

namespace GarageGroup.Infra.Dataverse.Api.Test;

partial class DataverseApiClientTest
{
    [Fact]
    public static async Task SendEmailAsync_InputIsNull_ExpectArgumentNullException()
    {
        var mockHttpApi = CreateMockEmailHttpApi(SomeEmailCreateJsonOut.InnerToJsonResponse(), default(DataverseJsonResponse));
        var dataverseApiClient = CreateDataverseApiClient(mockHttpApi.Object, CreateGuidProvider());

        var token = new CancellationToken(canceled: false);
        var ex = await Assert.ThrowsAsync<ArgumentNullException>(InnerSendEmailAsync);

        Assert.Equal("input", ex.ParamName);

        Task InnerSendEmailAsync()
            =>
            dataverseApiClient.SendEmailAsync(null!, token).AsTask();
    }

    [Fact]
    public static void SendEmailAsync_CancellationTokenIsCanceled_ExpectTaskIsCanceled()
    {
        var mockHttpApi = CreateMockEmailHttpApi(SomeEmailCreateJsonOut.InnerToJsonResponse(), default(DataverseJsonResponse));
        var dataverseApiClient = CreateDataverseApiClient(mockHttpApi.Object, CreateGuidProvider());

        var token = new CancellationToken(canceled: true);
        var actualTask = dataverseApiClient.SendEmailAsync(SomeEmailSendInWithEmailId, token);

        Assert.True(actualTask.IsCanceled);
    }

    [Theory]
    [MemberData(nameof(ApiClientTestDataSource.EmailSendInputTestData), MemberType = typeof(ApiClientTestDataSource))]
    internal static async Task SendEmailAsync_CancellationTokenIsNotCanceled_ExpectHttpRequestCalledOnce(
        DataverseEmailSendIn input,
        DataverseJsonRequest? expectedCreationRequest,
        DataverseJsonRequest expectedSendingRequest,
        DataverseEmailCreateJsonOut? createOut)
    {
        var emailOut = createOut ?? SomeEmailCreateJson;

        var mockHttpApi = CreateMockEmailHttpApi(
            emailOut.InnerToJsonResponse(),
            default(DataverseJsonResponse),
            OnCreateRequest);

        var dataverseApiClient = CreateDataverseApiClient(mockHttpApi.Object, CreateGuidProvider());

        var token = new CancellationToken(canceled: false);
        _ = await dataverseApiClient.SendEmailAsync(input, token);
        
        if (expectedCreationRequest is not null)
        {
            mockHttpApi.Verify(p => p.SendJsonAsync(It.IsAny<DataverseJsonRequest>(), token), Times.Exactly(2));
        }
        
        mockHttpApi.Verify(p => p.SendJsonAsync(expectedSendingRequest, token), Times.Once);

        void OnCreateRequest(DataverseJsonRequest actual)
            =>
            actual.ShouldDeepEqual(expectedCreationRequest);
    }

    [Fact]
    public static async Task SendEmailAsync_HttpApiEmailSendThrowsException_ExpectFailure()
    {
        var sourceException = new Exception("Some Error message");

        var mockHttpApi = CreateMockHttpApi(sourceException);
        var dataverseApiClient = CreateDataverseApiClient(mockHttpApi.Object, CreateGuidProvider());

        var input = SomeEmailSendInWithEmailId;
        var actual = await dataverseApiClient.SendEmailAsync(input, default);

        var expected = Failure.Create(
            DataverseFailureCode.Unknown,
            "An unexpected exception was thrown when trying to send a Dataverse email",
            sourceException);

        Assert.StrictEqual(expected, actual);
    }

    [Fact]
    public static async Task SendEmailAsync_HttpApiEmailCreateThrowsException_ExpectFailure()
    {
        var sourceException = new Exception("Some Error message");

        var mockHttpApi = CreateMockHttpApi(sourceException);
        var dataverseApiClient = CreateDataverseApiClient(mockHttpApi.Object, CreateGuidProvider());

        var input = SomeEmailSendInWithoutEmailId;
        var actual = await dataverseApiClient.SendEmailAsync(input, default);

        var expected = Failure.Create(
            DataverseFailureCode.Unknown,
            "An unexpected exception was thrown when trying to create a Dataverse email",
            sourceException);

        Assert.StrictEqual(expected, actual);
    }

    [Theory]
    [MemberData(nameof(ApiClientTestDataSource.FailureOutputTestData), MemberType = typeof(ApiClientTestDataSource))]
    public static async Task SendEmailAsync_ResponseIsFailure_ExpectFailureInEmailCreation(
        Failure<DataverseFailureCode> failure)
    {
        var mockHttpApi = CreateMockEmailHttpApi(failure, default(DataverseJsonResponse));
        var dataverseApiClient = CreateDataverseApiClient(mockHttpApi.Object, CreateGuidProvider());

        var input = SomeEmailSendInWithoutEmailId;
        var actual = await dataverseApiClient.SendEmailAsync(input, CancellationToken.None);
        
        Assert.StrictEqual(failure, actual);
    }

    [Theory]
    [MemberData(nameof(ApiClientTestDataSource.FailureOutputTestData), MemberType = typeof(ApiClientTestDataSource))]
    public static async Task SendEmailAsync_ResponseIsFailure_ExpectFailureInEmailSending(
        Failure<DataverseFailureCode> failure)
    {
        var mockHttpApi = CreateMockEmailHttpApi(SomeEmailCreateJson.InnerToJsonResponse(), failure);
        var dataverseApiClient = CreateDataverseApiClient(mockHttpApi.Object, CreateGuidProvider());
        
        var actual = await dataverseApiClient.SendEmailAsync(SomeEmailSendInWithEmailId, CancellationToken.None);
        
        Assert.StrictEqual(failure, actual);
    }

    [Fact]
    public static async Task SendEmailAsync_ResponseIsSuccessAndInputWithEmailId_ExpectSuccessWithInputEmailId()
    {
        var emailOut = new DataverseEmailCreateJsonOut
        {
            ActivityId = Guid.Parse("c09815c5-3670-478d-bd2b-b4df0de171a3")
        };

        var mockHttpApi = CreateMockEmailHttpApi(emailOut.InnerToJsonResponse(), default(DataverseJsonResponse));
        var dataverseApiClient = CreateDataverseApiClient(mockHttpApi.Object, CreateGuidProvider());

        var input = new DataverseEmailSendIn(
            emailId: Guid.Parse("ba7ac741-2b81-4caa-b25b-9cdbb43ca959"));

        var actual = await dataverseApiClient.SendEmailAsync(input, default);

        var expected = new DataverseEmailSendOut(
            emailId: Guid.Parse("ba7ac741-2b81-4caa-b25b-9cdbb43ca959"));

        Assert.StrictEqual(expected, actual);
    }

    [Fact]
    public static async Task SendEmailAsync_ResponseIsSuccessAndInputWithoutEmailId_ExpectSuccessWithCreatedEmailId()
    {
        var emailOut = new DataverseEmailCreateJsonOut
        {
            ActivityId = Guid.Parse("cf59c0e0-e358-4433-be06-321bbf08cb05")
        };

        var mockHttpApi = CreateMockEmailHttpApi(emailOut.InnerToJsonResponse(), default(DataverseJsonResponse));
        var dataverseApiClient = CreateDataverseApiClient(mockHttpApi.Object, CreateGuidProvider());

        var input = SomeEmailSendInWithoutEmailId;
        var actual = await dataverseApiClient.SendEmailAsync(input, default);

        var expected = new DataverseEmailSendOut(
            emailId: Guid.Parse("cf59c0e0-e358-4433-be06-321bbf08cb05"));

        Assert.StrictEqual(expected, actual);
    }
}