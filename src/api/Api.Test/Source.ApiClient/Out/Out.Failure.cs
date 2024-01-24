using System;
using Xunit;

namespace GarageGroup.Infra.Dataverse.Api.Test;

partial class ApiClientTestDataSource
{
    public static TheoryData<Failure<DataverseFailureCode>> FailureOutputTestData
        =>
        new()
        {
            {
                new(DataverseFailureCode.Unknown, string.Empty)
            },
            {
                new(DataverseFailureCode.Unknown, "Some text")
            },
            {
                new(DataverseFailureCode.Unauthorized, "Some unauthorized failure")
            },
            {
                new(DataverseFailureCode.RecordNotFound, "Some Failure Message")
            },
            {
                new(DataverseFailureCode.PicklistValueOutOfRange, "Error message")
            },
            {
                new(DataverseFailureCode.UserNotEnabled, "User was not found")
            },
            {
                new(DataverseFailureCode.PrivilegeDenied, "Some failure")
            },
            {
                new(DataverseFailureCode.Throttling, "Some Throttling Failure")
            },
            {
                new(DataverseFailureCode.SearchableEntityNotFound, "Some failure")
            },
            {
                new(DataverseFailureCode.InvalidPayload, "Some invalid data")
            }
        };
}