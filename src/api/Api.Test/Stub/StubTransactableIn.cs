namespace GarageGroup.Infra.Dataverse.Api.Test;

internal sealed record class StubTransactableIn<T> : IDataverseTransactableIn<T>
    where T : notnull
{
    public T? Entity
        =>
        default;
}