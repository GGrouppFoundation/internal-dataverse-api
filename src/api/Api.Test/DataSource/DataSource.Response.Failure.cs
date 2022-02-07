using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Mime;
using System.Text.Json;

namespace GGroupp.Infra.Dataverse.Api.Test;

partial class ApiClientTestDataSource
{
    public static IEnumerable<object?[]> GetFailureResponseTestDataPair()
    {
        yield return new object?[]
        {
            null,
            default(Failure<DataverseFailureCode>)
        };

        var content = new DataverseFailureJson
        {
            ErrorCode = "0x80060891",
            Message = "Some message"
        }
        .Serialize();

        yield return new object?[]
        {
            new StringContent(content),
            Failure.Create(DataverseFailureCode.Unknown, content)
        };

        var exceptionFailure = new DataverseFailureJson
        {
            ExceptionMessage = "Some exception message"
        };

        yield return new object?[]
        {
            exceptionFailure.ToJsonContent(),
            Failure.Create(DataverseFailureCode.Unknown, exceptionFailure.ExceptionMessage)
        };

        var recordNotFoundByEntityKeyFailure = new DataverseFailureJson
        {
            Error = new()
            {
                Code = "0x80060891",
                Description = "Some record was not found"
            }
        };

        yield return new object?[]
        {
            recordNotFoundByEntityKeyFailure.ToJsonContent(),
            Failure.Create(DataverseFailureCode.RecordNotFound, recordNotFoundByEntityKeyFailure.Error.Description)
        };

        var objectDoesNotExistFailure = new DataverseFailureJson
        {
            Failure = new DataverseFailureInfoJson
            {
                Code = "0x80040217",
                Message = "Some object does not exist"
            }
        };

        yield return new object?[]
        {
            objectDoesNotExistFailure.ToJsonContent(),
            Failure.Create(DataverseFailureCode.RecordNotFound, objectDoesNotExistFailure.Failure.Message)
        };

        var picklistValueOutOfRangeFailure = new DataverseFailureJson
        {
            ErrorCode = "0x8004431A",
            ExceptionMessage = "Some pick list value is out of range"
        };

        yield return new object?[]
        {
            picklistValueOutOfRangeFailure.ToJsonContent(),
            Failure.Create(DataverseFailureCode.PicklistValueOutOfRange, picklistValueOutOfRangeFailure.ExceptionMessage)
        };

        var privilegeDeniedFailure = new DataverseFailureJson
        {
            Error = new()
            {
                Code = "0x80040220",
                Description = "Some user does not hold the necessary privileges"
            }
        };

        yield return new object?[]
        {
            privilegeDeniedFailure.ToJsonContent(),
            Failure.Create(DataverseFailureCode.PrivilegeDenied, privilegeDeniedFailure.Error.Description)
        };

        var unManagedIdsAccessDeniedFailure = new DataverseFailureJson
        {
            Failure = new()
            {
                Code = "0x80048306",
                Message = "Not enough privilege. Some message"
            }
        };

        yield return new object?[]
        {
            unManagedIdsAccessDeniedFailure.ToJsonContent(),
            Failure.Create(DataverseFailureCode.PrivilegeDenied, unManagedIdsAccessDeniedFailure.Failure.Message)
        };

        var unManagedIdsUserNotEnabledFailure = new DataverseFailureJson
        {
            ErrorCode = "0x80040225",
            Message = "Some user is disabled"
        };

        yield return new object?[]
        {
            unManagedIdsUserNotEnabledFailure.ToJsonContent(),
            Failure.Create(DataverseFailureCode.UserNotEnabled, unManagedIdsUserNotEnabledFailure.Message)
        };

        var userNotAssignedLicenseFailure = new DataverseFailureJson
        {
            Error = new()
            {
                Code = "0x8004d24b",
                Description = "Some user does not have any License"
            }
        };

        yield return new object?[]
        {
            userNotAssignedLicenseFailure.ToJsonContent(),
            Failure.Create(DataverseFailureCode.UserNotEnabled, userNotAssignedLicenseFailure.Error.Description)
        };

        var searchableEntityNotFoundFailure = new DataverseFailureJson
        {
            Error = new()
            {
                Code = "SearchableEntityNotFound",
                Description = "Some entity was not found"
            }
        };

        yield return new object?[]
        {
            searchableEntityNotFoundFailure.ToJsonContent(),
            Failure.Create(DataverseFailureCode.SearchableEntityNotFound, searchableEntityNotFoundFailure.Error.Description)
        };

        var throttlingFailure = new DataverseFailureJson
        {
            Failure = new()
            {
                Code = "0x8005F103",
                Message = "Some throttling message"
            }
        };

        yield return new object?[]
        {
            throttlingFailure.ToJsonContent(),
            Failure.Create(DataverseFailureCode.Throttling, throttlingFailure.Failure.Message)
        };

        var throttlingBurstRequestLimitExceededErrorFailure = new DataverseFailureJson
        {
            ErrorCode = "0x80072322",
            Message = "Some throttling error message",
            ExceptionMessage = "Some throttling exception message"
        };

        yield return new object?[]
        {
            throttlingBurstRequestLimitExceededErrorFailure.ToJsonContent(),
            Failure.Create(DataverseFailureCode.Throttling, throttlingBurstRequestLimitExceededErrorFailure.Message)
        };

        var throttlingConcurrencyLimitExceededErrorFailure = new DataverseFailureJson
        {
            Error = new()
            {
                Code = "0x80072326",
                Description = "Some throttling concurrency limit was exceeded"
            }
        };

        yield return new object?[]
        {
            throttlingConcurrencyLimitExceededErrorFailure.ToJsonContent(),
            Failure.Create(DataverseFailureCode.Throttling, throttlingConcurrencyLimitExceededErrorFailure.Error.Description)
        };

        var throttlingTimeExceededErrorFailure = new DataverseFailureJson
        {
            Error = new()
            {
                Code = "0x80072321",
                Description = "Some throttling time was exceeded"
            }
        };

        yield return new object?[]
        {
            throttlingTimeExceededErrorFailure.ToJsonContent(),
            Failure.Create(DataverseFailureCode.Throttling, throttlingTimeExceededErrorFailure.Error.Description)
        };

        var throttlingCodeFailure = new DataverseFailureJson
        {
            Failure = new()
            {
                Code = "0x80060308",
                Message = "Some throttling failure"
            }
        };

        yield return new object?[]
        {
            throttlingCodeFailure.ToJsonContent(),
            Failure.Create(DataverseFailureCode.Throttling, throttlingCodeFailure.Failure.Message)
        };

        var unknownFailure = new DataverseFailureJson
        {
            Failure = new()
            {
                Code = "0x80060454",
                Message = "Some unknown failure"
            }
        };

        yield return new object?[]
        {
            unknownFailure.ToJsonContent(),
            Failure.Create(DataverseFailureCode.Unknown, unknownFailure.Failure.Message)
        };
    }

    private static StringContent ToJsonContent(this DataverseFailureJson failureJson)
        =>
        new(failureJson.Serialize(), default, MediaTypeNames.Application.Json);

    private static string Serialize(this DataverseFailureJson failureJson)
        =>
        JsonSerializer.Serialize(failureJson);
}