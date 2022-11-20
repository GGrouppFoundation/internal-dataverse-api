using System.Collections.Generic;
using System.Net;
using System.Net.Http;

namespace GGroupp.Infra.Dataverse.Api.Test;

partial class ApiClientTestDataSource
{
    public static IEnumerable<object?[]> GetUnauthorizedOutputTestData()
    {
        yield return new object?[]
        {
            null,
            HttpStatusCode.Unauthorized.GetDefaultFailureMessage()
        };

        var content = new StubFailureJson
        {
            ErrorCode = "0x80060891",
            Message = "Some message"
        }
        .Serialize();

        yield return new object?[]
        {
            new StringContent(content),
            content
        };

        var exceptionFailure = new StubFailureJson
        {
            ExceptionMessage = "Some exception message"
        };

        yield return new object?[]
        {
            exceptionFailure.ToJsonContent(),
            exceptionFailure.ExceptionMessage
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
            recordNotFoundByEntityKeyFailure.ToJsonContent(),
            recordNotFoundByEntityKeyFailure.Error.Description
        };

        var objectDoesNotExistFailure = new StubFailureJson
        {
            Failure = new()
            {
                Code = "0x80040217",
                Message = "Some object does not exist"
            }
        };

        yield return new object?[]
        {
            objectDoesNotExistFailure.ToJsonContent(),
            objectDoesNotExistFailure.Failure.Message
        };

        var picklistValueOutOfRangeFailure = new StubFailureJson
        {
            ErrorCode = "0x8004431A"
        };

        yield return new object?[]
        {
            picklistValueOutOfRangeFailure.ToJsonContent(),
            picklistValueOutOfRangeFailure.Serialize()
        };

        var privilegeDeniedFailure = new StubFailureJson
        {
            Error = new()
            {
                Code = "0x80040220"
            }
        };

        yield return new object?[]
        {
            privilegeDeniedFailure.ToJsonContent(),
            privilegeDeniedFailure.Serialize()
        };

        var unManagedIdsAccessDeniedFailure = new StubFailureJson
        {
            Failure = new()
            {
                Code = "0x80048306"
            }
        };

        yield return new object?[]
        {
            unManagedIdsAccessDeniedFailure.ToJsonContent(),
            unManagedIdsAccessDeniedFailure.Serialize()
        };
    }
}