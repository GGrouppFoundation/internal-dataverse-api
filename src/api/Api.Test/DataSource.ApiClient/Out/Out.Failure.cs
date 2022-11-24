using System;
using System.Collections.Generic;

namespace GGroupp.Infra.Dataverse.Api.Test;

partial class ApiClientTestDataSource
{
    public static IEnumerable<object?[]> GetFailureOutputTestData()
        =>
        new[]
        {
            new object?[]
            {
                Failure.Create(DataverseFailureCode.Unknown, string.Empty)
            },
            new object?[]
            {
                Failure.Create(DataverseFailureCode.Unknown, "Some text")
            },
            new object?[]
            {
                Failure.Create(DataverseFailureCode.Unauthorized, "Some unauthorized failure")
            },
            new object?[]
            {
                Failure.Create(DataverseFailureCode.RecordNotFound, "Some Failure Message")
            },
            new object?[]
            {
                Failure.Create(DataverseFailureCode.PicklistValueOutOfRange, "Error message")
            },
            new object?[]
            {
                Failure.Create(DataverseFailureCode.UserNotEnabled, "User was not found")
            },
            new object?[]
            {
                Failure.Create(DataverseFailureCode.PrivilegeDenied, "Some failure")
            },
            new object?[]
            {
                Failure.Create(DataverseFailureCode.Throttling, "Some Throttling Failure")
            },
            new object?[]
            {
                Failure.Create(DataverseFailureCode.SearchableEntityNotFound, "Some failure")
            }
        };
}