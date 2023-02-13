using System;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Xunit;

namespace GGroupp.Infra.Dataverse.Api.Test;

partial class DataverseApiClientTest
{
    [Fact]
    public static void WhoAmIAsync_CancellationTokenIsCanceled_ExpectTaskIsCanceled()
    {
        var mockHttpApi = CreateMockHttpApi<Unit, DataverseWhoAmIOutJson>(SomeWhoAmIOutJson);
        var dataverseApiClient = CreateDataverseApiClient(mockHttpApi.Object);

        var token = new CancellationToken(canceled: true);
        var actualTask = dataverseApiClient.WhoAmIAsync(default, token);

        Assert.True(actualTask.IsCanceled);
    }

    [Theory]
    [MemberData(nameof(ApiClientTestDataSource.WhoAmIInputTestData), MemberType = typeof(ApiClientTestDataSource))]
    internal static async Task WhoAmIAsync_CancellationTokenIsNotCanceled_ExpectHttpRequestCalledOnce(
        Guid? callerId, DataverseHttpRequest<Unit> expectedRequest)
    {
        var mockHttpApi = CreateMockHttpApi<Unit, DataverseWhoAmIOutJson>(SomeWhoAmIOutJson);
        var dataverseApiClient = CreateDataverseApiClient(mockHttpApi.Object, callerId);

        var token = new CancellationToken(canceled: false);
        _ = await dataverseApiClient.WhoAmIAsync(default, token);

        mockHttpApi.Verify(p => p.InvokeAsync<Unit, DataverseWhoAmIOutJson>(expectedRequest, token), Times.Once);
    }

    [Theory]
    [MemberData(nameof(ApiClientTestDataSource.FailureOutputTestData), MemberType = typeof(ApiClientTestDataSource))]
    public static async Task WhoAmIAsync_ResponseIsFailure_ExpectFailure(
        Failure<DataverseFailureCode> failure)
    {
        var mockHttpApi = CreateMockHttpApi<Unit, DataverseWhoAmIOutJson>(failure);
        var dataverseApiClient = CreateDataverseApiClient(mockHttpApi.Object);

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

        var mockHttpApi = CreateMockHttpApi<Unit, DataverseWhoAmIOutJson>(success);
        var dataverseApiClient = CreateDataverseApiClient(mockHttpApi.Object);

        var actual = await dataverseApiClient.WhoAmIAsync(default, CancellationToken.None);

        var expected = new DataverseWhoAmIOut(
            businessUnitId: Guid.Parse("51ea96d6-5119-4059-b649-d90c0a4aeab6"),
            userId: Guid.Parse("6ac49276-357d-441f-89c4-c118cbaa5ee3"),
            organizationId: Guid.Parse("92847989-5772-4ff9-851b-661dc66720ae"));

        Assert.StrictEqual(expected, actual);
    }
}