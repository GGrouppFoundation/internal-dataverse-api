using System;
using System.Text.Json;

namespace GGroupp.Infra;

public sealed record DataverseSearchJsonValue
{
    public JsonElement Value { get; }

    public JsonValueKind Kind {  get; }

    public DataverseSearchJsonValue(JsonElement jsonElement)
    {
        Value = jsonElement;
        Kind = jsonElement.ValueKind;
    }

    public int? ToInt32()
        =>
        Kind switch
        {
            JsonValueKind.Null => null,
            JsonValueKind.Number => Value.GetInt32(),
            _ => throw CreateInvalidOperationException(nameof(JsonValueKind.Number), Kind)
        };

    public override string? ToString()
        =>
        Kind switch
        {
            JsonValueKind.Null => null,
            JsonValueKind.String => Value.ToStringOrEmpty(),
            _ => throw CreateInvalidOperationException(nameof(JsonValueKind.String), Kind)
        };

    public double? ToDouble()
        =>
        Kind switch
        {
            JsonValueKind.Null => null,
            _ when Value.TryGetDouble(out var resultDoubleValue) => resultDoubleValue,
            _ => throw CreateInvalidOperationException("double", Kind)
        };

    public DateTime? ToDateTime()
        =>
        Kind switch
        {
            JsonValueKind.Null => null,
            JsonValueKind.String when DateTime.TryParse(Value.ToStringOrEmpty(), out var dateTimeResult) => dateTimeResult,
            _ => throw CreateInvalidOperationException(nameof(JsonValueKind.String), Kind)
        };

    public Guid? ToGuid() 
        =>
        Kind switch
        {
            JsonValueKind.Null => null,
            JsonValueKind.String when Value.TryGetGuid(out var guidResult) => guidResult,
            _ => throw CreateInvalidOperationException(nameof(JsonValueKind.String), Kind)
        };

    private static InvalidOperationException CreateInvalidOperationException(string expectedType, JsonValueKind actualType)
        =>
        new($"The type was expected to be {expectedType}, but actual type is {actualType}");
}
