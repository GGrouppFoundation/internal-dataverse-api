namespace GarageGroup.Infra.Dataverse.Api.Test;

internal sealed record class StubRequestJson : IStubRequestJson
{
    public int Id { get; init; }

    public string? Name { get; init; }
}