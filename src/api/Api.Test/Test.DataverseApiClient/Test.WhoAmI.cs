using System;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Xunit;

namespace GarageGroup.Infra.Dataverse.Api.Test;

partial class DataverseApiClientTest
{
    [Fact]
    public static void WhoAmIAsync_CancellationTokenIsCanceled_ExpectTaskIsCanceled()
    {
        var mockHttpApi = CreateMockJsonHttpApi(SomeWhoAmIOutJson.InnerToJsonResponse());
        var dataverseApiClient = CreateDataverseApiClient(mockHttpApi.Object, CreateGuidProvider());

        var token = new CancellationToken(canceled: true);
        var actualTask = dataverseApiClient.WhoAmIAsync(default, token);

        Assert.True(actualTask.IsCanceled);
    }

    [Theory]
    [MemberData(nameof(ApiClientTestDataSource.WhoAmIInputTestData), MemberType = typeof(ApiClientTestDataSource))]
    internal static async Task WhoAmIAsync_CancellationTokenIsNotCanceled_ExpectHttpRequestCalledOnce(
        Guid? callerId, DataverseJsonRequest expectedRequest)
    {
        var mockHttpApi = CreateMockJsonHttpApi(SomeWhoAmIOutJson.InnerToJsonResponse());
        var dataverseApiClient = CreateDataverseApiClient(mockHttpApi.Object, CreateGuidProvider(), callerId);

        var token = new CancellationToken(canceled: false);
        _ = await dataverseApiClient.WhoAmIAsync(default, token);

        mockHttpApi.Verify(p => p.SendJsonAsync(expectedRequest, token), Times.Once);
    }

    [Fact]
    public static async Task WhoAmIAsync_HttpApiThrowsException_ExpectFailure()
    {
        var sourceException = new Exception("Some exception message");

        var mockHttpApi = CreateMockHttpApi(sourceException);
        var dataverseApiClient = CreateDataverseApiClient(mockHttpApi.Object, CreateGuidProvider());

        var actual = await dataverseApiClient.WhoAmIAsync(default, CancellationToken.None);

        var expected = Failure.Create(
            DataverseFailureCode.Unknown,
            "An unexpected exception was thrown when trying to get a Dataverse current user data",
            sourceException);

        Assert.StrictEqual(expected, actual);
    }

    [Theory]
    [MemberData(nameof(ApiClientTestDataSource.FailureOutputTestData), MemberType = typeof(ApiClientTestDataSource))]
    public static async Task WhoAmIAsync_ResponseIsFailure_ExpectFailure(
        Failure<DataverseFailureCode> failure)
    {
        var mockHttpApi = CreateMockJsonHttpApi(failure);
        var dataverseApiClient = CreateDataverseApiClient(mockHttpApi.Object, CreateGuidProvider());

        var actual = await dataverseApiClient.WhoAmIAsync(default, CancellationToken.None);
        Assert.StrictEqual(failure, actual);
    }

    [Fact]
    public static async Task WhoAmIAsync_ResponseIsSuccess_ExpectSuccess()
    {
        var success = new DataverseWhoAmIOutJson
        {
            BusinessUnitId = Guid.Parse("51ea96d6-5119-4059-b649-d90c0a4aeab6"),
            UserId = Guid.Parse("6ac49276-357d-441f-89c4-c118cbaa5ee3"),
            OrganizationId = Guid.Parse("92847989-5772-4ff9-851b-661dc66720ae")
        };

        var mockHttpApi = CreateMockJsonHttpApi(success.InnerToJsonResponse());
        var dataverseApiClient = CreateDataverseApiClient(mockHttpApi.Object, CreateGuidProvider());

        var actual = await dataverseApiClient.WhoAmIAsync(default, CancellationToken.None);

        var expected = new DataverseWhoAmIOut(
            businessUnitId: Guid.Parse("51ea96d6-5119-4059-b649-d90c0a4aeab6"),
            userId: Guid.Parse("6ac49276-357d-441f-89c4-c118cbaa5ee3"),
            organizationId: Guid.Parse("92847989-5772-4ff9-851b-661dc66720ae"));

        Assert.StrictEqual(expected, actual);
    }
}