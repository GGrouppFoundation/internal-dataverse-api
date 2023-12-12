﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Xunit;

namespace GarageGroup.Infra.Dataverse.Api.Test;

partial class DataverseApiClientTest
{
    [Fact]
    public static void PingAsync_CancellationTokenIsCanceled_ExpectTaskIsCanceled()
    {
        var mockHttpApi = CreateMockJsonHttpApi(SomeWhoAmIOutJson.InnerToJsonResponse());
        var dataverseApiClient = CreateDataverseApiClient(mockHttpApi.Object, CreateGuidProvider());

        var token = new CancellationToken(canceled: true);
        var actualTask = dataverseApiClient.PingAsync(default, token);

        Assert.True(actualTask.IsCanceled);
    }

    [Theory]
    [MemberData(nameof(ApiClientTestDataSource.PingInputTestData), MemberType = typeof(ApiClientTestDataSource))]
    internal static async Task PingAsync_CancellationTokenIsNotCanceled_ExpectHttpRequestCalledOnce(
        Guid? callerId, DataverseJsonRequest expectedRequest)
    {
        var mockHttpApi = CreateMockJsonHttpApi(SomeWhoAmIOutJson.InnerToJsonResponse());
        var dataverseApiClient = CreateDataverseApiClient(mockHttpApi.Object, CreateGuidProvider(), callerId);

        var token = new CancellationToken(canceled: false);
        _ = await dataverseApiClient.PingAsync(default, token);

        mockHttpApi.Verify(p => p.SendJsonAsync(expectedRequest, token), Times.Once);
    }

    [Fact]
    public static async Task PingAsync_HttpApiThrowsException_ExpectFailure()
    {
        var sourceException = new Exception("Some error message");

        var mockHttpApi = CreateMockHttpApi(sourceException);
        var dataverseApiClient = CreateDataverseApiClient(mockHttpApi.Object, CreateGuidProvider());

        var actual = await dataverseApiClient.PingAsync(default, CancellationToken.None);

        var expected = Failure.Create(
            "An unexpected exception was thrown when trying to ping a Dataverse API",
            sourceException);

        Assert.StrictEqual(expected, actual);
    }

    [Theory]
    [MemberData(nameof(ApiClientTestDataSource.FailureOutputTestData), MemberType = typeof(ApiClientTestDataSource))]
    public static async Task PingAsync_ResponseIsFailure_ExpectFailure(
        Failure<DataverseFailureCode> failure)
    {
        var mockHttpApi = CreateMockJsonHttpApi(failure);
        var dataverseApiClient = CreateDataverseApiClient(mockHttpApi.Object, CreateGuidProvider());

        var actual = await dataverseApiClient.PingAsync(default, CancellationToken.None);
        var expected = Failure.Create(failure.FailureMessage, failure.SourceException);

        Assert.StrictEqual(expected, actual);
    }

    [Fact]
    public static async Task PingAsync_ResponseIsSuccess_ExpectSuccess()
    {
        var success = new DataverseWhoAmIOutJson
        {
            BusinessUnitId = Guid.Parse("7b91a1c5-0c9e-4f61-a604-11bffbed4d3d"),
            UserId = Guid.Parse("c0ddd988-9085-47a6-bb84-08bbb8bd7424"),
            OrganizationId = Guid.Parse("7ff765f3-3d0c-45bd-ba6c-f5646dd0b838")
        };

        var mockHttpApi = CreateMockJsonHttpApi(success.InnerToJsonResponse());
        var dataverseApiClient = CreateDataverseApiClient(mockHttpApi.Object, CreateGuidProvider());

        var actual = await dataverseApiClient.PingAsync(default, CancellationToken.None);
        var expected = Result.Success<Unit>(default);

        Assert.StrictEqual(expected, actual);
    }
}