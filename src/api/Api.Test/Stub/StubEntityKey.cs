namespace GarageGroup.Infra.Dataverse.Api.Test;

internal sealed class StubEntityKey : IDataverseEntityKey
{
    internal StubEntityKey(string value)
        =>
        Value = value;

    public string Value { get; }
}