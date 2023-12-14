using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;

namespace GarageGroup.Infra.Dataverse.Api.Test;

partial class HttpApiTestDataSource
{
    public static IEnumerable<object?[]> FailureTestData
    {
        get
        {
            yield return new object?[]
            {
                HttpStatusCode.NotFound,
                null,
                Failure.Create(DataverseFailureCode.Unknown, HttpStatusCode.NotFound.GetDefaultFailureMessage())
            };

            var content = new StubFailureJson
            {
                ErrorCode = "0x80060891",
                Message = "Some message"
            }
            .Serialize();

            yield return new object?[]
            {
                HttpStatusCode.InternalServerError,
                new StringContent(content),
                Failure.Create(DataverseFailureCode.Unknown, content)
            };

            var exceptionFailure = new StubFailureJson
            {
                ExceptionMessage = "Some exception message"
            };

            yield return new object?[]
            {
                HttpStatusCode.BadRequest,
                exceptionFailure.ToJsonContent(),
                Failure.Create(DataverseFailureCode.Unknown, exceptionFailure.ExceptionMessage)
            };

            var recordNotFoundByEntityKeyFailure = new StubFailureJson
            {
                Error = new()
                {
                    Code = "0x80060891",
                    Description = "Some record was not found"
                }
            };

            yield return new object?[]
            {
                HttpStatusCode.TooManyRequests,
                recordNotFoundByEntityKeyFailure.ToJsonContent(),
                Failure.Create(DataverseFailureCode.RecordNotFound, recordNotFoundByEntityKeyFailure.Error.Description)
            };

            var objectDoesNotExistFailure = new StubFailureJson
            {
                Failure = new StubFailureInfoJson
                {
                    Code = "0x80040217",
                    Message = "Some object does not exist"
                }
            };

            yield return new object?[]
            {

                HttpStatusCode.Forbidden,
                objectDoesNotExistFailure.ToJsonContent(),
                Failure.Create(DataverseFailureCode.RecordNotFound, objectDoesNotExistFailure.Failure.Message)
            };

            var picklistValueOutOfRangeFailure = new StubFailureJson
            {
                ErrorCode = "0x8004431A",
                ExceptionMessage = "Some pick list value is out of range"
            };

            yield return new object?[]
            {

                HttpStatusCode.BadRequest,
                picklistValueOutOfRangeFailure.ToJsonContent(),
                Failure.Create(DataverseFailureCode.PicklistValueOutOfRange, picklistValueOutOfRangeFailure.ExceptionMessage)
            };

            var privilegeDeniedFailure = new StubFailureJson
            {
                Error = new()
                {
                    Code = "0x80040220",
                    Description = "Some user does not hold the necessary privileges"
                }
            };

            yield return new object?[]
            {
                HttpStatusCode.NotAcceptable,
                privilegeDeniedFailure.ToJsonContent(),
                Failure.Create(DataverseFailureCode.PrivilegeDenied, privilegeDeniedFailure.Error.Description)
            };

            var unManagedIdsAccessDeniedFailure = new StubFailureJson
            {
                Failure = new()
                {
                    Code = "0x80048306",
                    Message = "Not enough privilege. Some message"
                }
            };

            yield return new object?[]
            {
                HttpStatusCode.InternalServerError,
                unManagedIdsAccessDeniedFailure.ToJsonContent(),
                Failure.Create(DataverseFailureCode.PrivilegeDenied, unManagedIdsAccessDeniedFailure.Failure.Message)
            };

            var unManagedIdsUserNotEnabledFailure = new StubFailureJson
            {
                ErrorCode = "0x80040225",
                Message = "Some user is disabled"
            };

            yield return new object?[]
            {
                HttpStatusCode.NotFound,
                unManagedIdsUserNotEnabledFailure.ToJsonContent(),
                Failure.Create(DataverseFailureCode.UserNotEnabled, unManagedIdsUserNotEnabledFailure.Message)
            };

            var userNotAssignedLicenseFailure = new StubFailureJson
            {
                Error = new()
                {
                    Code = "0x8004d24b",
                    Description = "Some user does not have any License"
                }
            };

            yield return new object?[]
            {
                HttpStatusCode.Conflict,
                userNotAssignedLicenseFailure.ToJsonContent(),
                Failure.Create(DataverseFailureCode.UserNotEnabled, userNotAssignedLicenseFailure.Error.Description)
            };

            var searchableEntityNotFoundFailure = new StubFailureJson
            {
                Error = new()
                {
                    Code = "SearchableEntityNotFound",
                    Description = "Some entity was not found"
                }
            };

            yield return new object?[]
            {
                HttpStatusCode.NotFound,
                searchableEntityNotFoundFailure.ToJsonContent(),
                Failure.Create(DataverseFailureCode.SearchableEntityNotFound, searchableEntityNotFoundFailure.Error.Description)
            };

            var throttlingFailure = new StubFailureJson
            {
                Failure = new()
                {
                    Code = "0x8005F103",
                    Message = "Some throttling message"
                }
            };

            yield return new object?[]
            {
                HttpStatusCode.InternalServerError,
                throttlingFailure.ToJsonContent(),
                Failure.Create(DataverseFailureCode.Throttling, throttlingFailure.Failure.Message)
            };

            var throttlingBurstRequestLimitExceededErrorFailure = new StubFailureJson
            {
                ErrorCode = "0x80072322",
                Message = "Some throttling error message",
                ExceptionMessage = "Some throttling exception message"
            };

            yield return new object?[]
            {
                HttpStatusCode.BadRequest,
                throttlingBurstRequestLimitExceededErrorFailure.ToJsonContent(),
                Failure.Create(DataverseFailureCode.Throttling, throttlingBurstRequestLimitExceededErrorFailure.Message)
            };

            var throttlingConcurrencyLimitExceededErrorFailure = new StubFailureJson
            {
                Error = new()
                {
                    Code = "0x80072326",
                    Description = "Some throttling concurrency limit was exceeded"
                }
            };

            yield return new object?[]
            {
                HttpStatusCode.BadGateway,
                throttlingConcurrencyLimitExceededErrorFailure.ToJsonContent(),
                Failure.Create(DataverseFailureCode.Throttling, throttlingConcurrencyLimitExceededErrorFailure.Error.Description)
            };

            var throttlingTimeExceededErrorFailure = new StubFailureJson
            {
                Error = new()
                {
                    Code = "0x80072321",
                    Description = "Some throttling time was exceeded"
                }
            };

            yield return new object?[]
            {
                HttpStatusCode.PaymentRequired,
                throttlingTimeExceededErrorFailure.ToJsonContent(),
                Failure.Create(DataverseFailureCode.Throttling, throttlingTimeExceededErrorFailure.Error.Description)
            };

            var throttlingCodeFailure = new StubFailureJson
            {
                Failure = new()
                {
                    Code = "0x80060308",
                    Message = "Some throttling failure"
                }
            };

            yield return new object?[]
            {
                HttpStatusCode.ExpectationFailed,
                throttlingCodeFailure.ToJsonContent(),
                Failure.Create(DataverseFailureCode.Throttling, throttlingCodeFailure.Failure.Message)
            };

            var duplicateRecordsFoundFailure = new StubFailureJson
            {
                Failure = new()
                {
                    Code = "0x80040333",
                    Message = "A record was not created or updated because a duplicate of the current record already exists."
                }
            };

            yield return new object?[]
            {
                HttpStatusCode.InternalServerError,
                duplicateRecordsFoundFailure.ToJsonContent(),
                Failure.Create(DataverseFailureCode.DuplicateRecord, duplicateRecordsFoundFailure.Failure.Message)
            };

            var duplicateRecordEntityKeyFailure = new StubFailureJson
            {
                Error = new()
                {
                    Code = "0x80060892",
                    Description = "Some duplicate record failure message"
                }
            };

            yield return new object?[]
            {
                HttpStatusCode.BadRequest,
                duplicateRecordEntityKeyFailure.ToJsonContent(),
                Failure.Create(DataverseFailureCode.DuplicateRecord, duplicateRecordEntityKeyFailure.Error.Description)
            };

            var clientPayloadFailure = new StubFailureJson
            {
                Failure = new()
                {
                    Code = "0x80048d19",
                    Message = "Error identified in Payload provided by the user for Entity :''"
                }
            };

            yield return new object?[]
            {
                HttpStatusCode.BadRequest,
                clientPayloadFailure.ToJsonContent(),
                Failure.Create(DataverseFailureCode.InvalidPayload, "Error identified in Payload provided by the user for Entity :''")
            };

            var invalidArgumentFailure = new StubFailureJson
            {
                Failure = new()
                {
                    Code = "0x80040203",
                    Message = "Invalid argument."
                }
            };

            yield return new object?[]
            {
                HttpStatusCode.BadRequest,
                invalidArgumentFailure.ToJsonContent(),
                Failure.Create(DataverseFailureCode.InvalidPayload, "Invalid argument.")
            };

            var recipientEmailNotFoundFailure = new StubFailureJson
            {
                Failure = new()
                {
                    Code = "0x80040b0a",
                    Message = "Recipient email not found failure"
                }
            };

            yield return new object?[]
            {
                HttpStatusCode.BadRequest,
                recipientEmailNotFoundFailure.ToJsonContent(),
                Failure.Create(DataverseFailureCode.RecipientEmailNotFound, recipientEmailNotFoundFailure.Failure.Message)
            };

            var unknownFailure = new StubFailureJson
            {
                Failure = new()
                {
                    Code = "0x80060454",
                    Message = "Some unknown failure"
                }
            };

            yield return new object?[]
            {
                HttpStatusCode.BadRequest,
                unknownFailure.ToJsonContent(),
                Failure.Create(DataverseFailureCode.Unknown, unknownFailure.Failure.Message)
            };
        }
    }
}