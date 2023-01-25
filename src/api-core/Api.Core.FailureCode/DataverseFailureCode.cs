namespace GGroupp.Infra;

public enum DataverseFailureCode
{
    Unknown,

    Unauthorized,

    RecordNotFound,

    PicklistValueOutOfRange,

    UserNotEnabled,

    PrivilegeDenied,

    Throttling,

    SearchableEntityNotFound,

    DuplicateRecord
}