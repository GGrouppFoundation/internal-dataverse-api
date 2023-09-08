using System;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Xunit;

namespace GarageGroup.Infra.Dataverse.Api.Test;

partial class DataverseApiClientTest
{
    [Fact]
    public static void ExecuteChangeSetAsyncWithTOut_CancellationTokenIsCanceled_ExpectTaskIsCanceled()
    {
        var mockHttpApi = CreateMockChangeSetHttpApi(SomeChangeSetResponse);
        var dataverseApiClient = CreateDataverseApiClient(mockHttpApi.Object, CreateGuidProvider());

        var token = new CancellationToken(canceled: true);
        var input = SomeChangeSetInput;

        var actualTask = dataverseApiClient.ExecuteChangeSetAsync<object, StubResponseJson>(input, token);

        Assert.True(actualTask.IsCanceled);
    }

    [Fact]
    public static async Task ExecuteChangeSetAsyncWithTOut_InputIsEmpty_ExpectHttpApiCalledNever()
    {
        var mockHttpApi = CreateMockChangeSetHttpApi(SomeChangeSetResponse);
        var dataverseApiClient = CreateDataverseApiClient(mockHttpApi.Object, CreateGuidProvider());

        _ = await dataverseApiClient.ExecuteChangeSetAsync<object, StubResponseJson>(default, default);

        mockHttpApi.Verify(
            a => a.SendChangeSetAsync(It.IsAny<DataverseChangeSetRequest>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public static async Task ExecuteChangeSetAsyncWithTOut_InputIsEmpty_ExpectSuccessEmpty()
    {
        var mockHttpApi = CreateMockChangeSetHttpApi(SomeChangeSetResponse);
        var dataverseApiClient = CreateDataverseApiClient(mockHttpApi.Object, CreateGuidProvider());

        var actual = await dataverseApiClient.ExecuteChangeSetAsync<object, StubResponseJson>(default, default);
        var expected = default(DataverseChangeSetExecuteOut<StubResponseJson>);

        Assert.StrictEqual(expected, actual);
    }

    [Fact]
    public static async Task ExecuteChangeSetAsyncWithTOut_InputContainsInvalidRequestType_ExpectUnknownFailure()
    {
        var mockHttpApi = CreateMockChangeSetHttpApi(SomeChangeSetResponse);
        var dataverseApiClient = CreateDataverseApiClient(mockHttpApi.Object, CreateGuidProvider());

        var input = new DataverseChangeSetExecuteIn<StubRequestJson>(
            requests: new FlatArray<IDataverseTransactableIn<StubRequestJson>>(
                SomeDataverseEntityCreateInput,
                new StubTransactableIn<StubRequestJson>()));

        var actual = await dataverseApiClient.ExecuteChangeSetAsync<StubRequestJson, StubResponseJson>(input, default);
        Assert.True(actual.IsFailure);

        var actualFailure = actual.FailureOrThrow();
        Assert.StrictEqual(DataverseFailureCode.Unknown, actualFailure.FailureCode);

        var actualExceptionMessage = actualFailure.SourceException?.Message;
        var expectedExceptionMessage = $"An unexpected change set type: '{typeof(StubTransactableIn<StubRequestJson>).FullName}'";

        Assert.Equal(expectedExceptionMessage, actualExceptionMessage);
    }

    [Theory]
    [MemberData(nameof(ApiClientTestDataSource.ChangeSetExecuteWithTOutInputTestData), MemberType = typeof(ApiClientTestDataSource))]
    internal static async Task ExecuteChangeSetAsyncWithTOut_InputDoesNotContainInvalidRequestType_ExpectHttpRequestCalledOnce(
        Guid? callerId, Guid batchId, Guid changeSetId, DataverseChangeSetExecuteIn<object> input, DataverseChangeSetRequest expectedRequest)
    {
        var mockHttpApi = CreateMockChangeSetHttpApi(SomeChangeSetResponse);
        var guidProvider = CreateGuidProvider(batchId, changeSetId);

        var dataverseApiClient = CreateDataverseApiClient(mockHttpApi.Object, guidProvider, callerId);

        var token = new CancellationToken(canceled: false);
        _ = await dataverseApiClient.ExecuteChangeSetAsync<object, StubResponseJson>(input, token);

        mockHttpApi.Verify(p => p.SendChangeSetAsync(expectedRequest, token), Times.Once);
    }

    [Fact]
    public static async Task ExecuteChangeSetAsyncWithTOut_HttpApiThrowsException_ExpectFailure()
    {
        var sourceException = new InvalidOperationException("Some exception message");

        var mockHttpApi = CreateMockHttpApi(sourceException);
        var dataverseApiClient = CreateDataverseApiClient(mockHttpApi.Object, CreateGuidProvider());

        var input = SomeChangeSetInput;
        var actual = await dataverseApiClient.ExecuteChangeSetAsync<object, StubResponseJson>(input, default);

        var expected = Failure.Create(
            DataverseFailureCode.Unknown,
            "An unexpected exception was thrown when trying to execute a Dataverse change set",
            sourceException);

        Assert.StrictEqual(expected, actual);
    }

    [Theory]
    [MemberData(nameof(ApiClientTestDataSource.FailureOutputTestData), MemberType = typeof(ApiClientTestDataSource))]
    public static async Task ExecuteChangeSetAsyncWithTOut_ResponseIsFailure_ExpectFailure(
        Failure<DataverseFailureCode> failure)
    {
        var mockHttpApi = CreateMockChangeSetHttpApi(failure);
        var dataverseApiClient = CreateDataverseApiClient(mockHttpApi.Object, CreateGuidProvider());

        var input = SomeChangeSetInput;
        var actual = await dataverseApiClient.ExecuteChangeSetAsync<object, StubResponseJson>(input, default);

        Assert.StrictEqual(failure, actual);
    }

    [Fact]
    public static async Task ExecuteChangeSetAsyncWithTOut_ResponseIsSuccessEmpty_ExpectSuccessEmpty()
    {
        var mockHttpApi = CreateMockChangeSetHttpApi(default(DataverseChangeSetResponse));
        var dataverseApiClient = CreateDataverseApiClient(mockHttpApi.Object, CreateGuidProvider());

        var input = SomeChangeSetInput;

        var actual = await dataverseApiClient.ExecuteChangeSetAsync<object, StubResponseJson>(input, default);
        var expected = default(DataverseChangeSetExecuteOut<StubResponseJson>);

        Assert.StrictEqual(expected, actual);
    }

    [Fact]
    public static async Task ExecuteChangeSetAsyncWithTOut_ResponseIsSuccess_ExpectSuccess()
    {
        var changeSetResponse = new DataverseChangeSetResponse(
            responses: new[]
            {
                default,
                SomeResponseJson.InnerToJsonResponse(),
                new DataverseJsonResponse(
                    content: new(string.Empty))
            });

        var mockHttpApi = CreateMockChangeSetHttpApi(changeSetResponse);
        var dataverseApiClient = CreateDataverseApiClient(mockHttpApi.Object, CreateGuidProvider());

        var input = SomeChangeSetInput;
        var actual = await dataverseApiClient.ExecuteChangeSetAsync<object, StubResponseJson>(input, default);

        var expected = new DataverseChangeSetExecuteOut<StubResponseJson>(
            values: new(null, SomeResponseJson, null));

        Assert.StrictEqual(expected, actual);
    }
}