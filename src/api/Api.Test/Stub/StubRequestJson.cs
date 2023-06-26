namespace GarageGroup.Infra.Dataverse.Api.Test;

public sealed record class StubRequestJson
{
    public int Id { get; init; }

    public string? Name { get; init; }
}