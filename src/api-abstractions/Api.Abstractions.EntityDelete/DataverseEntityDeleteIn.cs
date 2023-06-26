namespace GarageGroup.Infra;

public sealed record class DataverseEntityDeleteIn
{
    public DataverseEntityDeleteIn(string entityPluralName, IDataverseEntityKey entityKey)
    {
        EntityPluralName = entityPluralName ?? string.Empty;
        EntityKey = entityKey;
    }

    public string EntityPluralName { get; }

    public IDataverseEntityKey EntityKey{ get; }
}
