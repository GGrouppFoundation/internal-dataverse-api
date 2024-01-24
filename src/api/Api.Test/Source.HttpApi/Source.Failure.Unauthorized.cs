using System.Net;
using System.Net.Http;
using Xunit;

namespace GarageGroup.Infra.Dataverse.Api.Test;

partial class HttpApiTestDataSource
{
    public static TheoryData<StringContent?, string> UnauthorizedTestData
    {
        get
        {
            var data = new TheoryData<StringContent?, string>
            {
                {
                    null,
                    HttpStatusCode.Unauthorized.GetDefaultFailureMessage()
                }
            };

            var content = new StubFailureJson
            {
                ErrorCode = "0x80060891",
                Message = "Some message"
            }
            .Serialize();

            data.Add(
                new(content),
                content);

            var exceptionFailure = new StubFailureJson
            {
                ExceptionMessage = "Some exception message"
            };

            data.Add(
                exceptionFailure.ToJsonContent(),
                exceptionFailure.ExceptionMessage);

            var recordNotFoundByEntityKeyFailure = new StubFailureJson
            {
                Error = new()
                {
                    Code = "0x80060891",
                    Description = "Some record was not found"
                }
            };

            data.Add(
                recordNotFoundByEntityKeyFailure.ToJsonContent(),
                recordNotFoundByEntityKeyFailure.Error.Description);

            var objectDoesNotExistFailure = new StubFailureJson
            {
                Failure = new()
                {
                    Code = "0x80040217",
                    Message = "Some object does not exist"
                }
            };

            data.Add(
                objectDoesNotExistFailure.ToJsonContent(),
                objectDoesNotExistFailure.Failure.Message);

            var picklistValueOutOfRangeFailure = new StubFailureJson
            {
                ErrorCode = "0x8004431A"
            };

            data.Add(
                picklistValueOutOfRangeFailure.ToJsonContent(),
                picklistValueOutOfRangeFailure.Serialize());

            var privilegeDeniedFailure = new StubFailureJson
            {
                Error = new()
                {
                    Code = "0x80040220"
                }
            };

            data.Add(
                privilegeDeniedFailure.ToJsonContent(),
                privilegeDeniedFailure.Serialize());

            var unManagedIdsAccessDeniedFailure = new StubFailureJson
            {
                Failure = new()
                {
                    Code = "0x80048306"
                }
            };

            data.Add(
                unManagedIdsAccessDeniedFailure.ToJsonContent(),
                unManagedIdsAccessDeniedFailure.Serialize());

            return data;
        }
    }
}