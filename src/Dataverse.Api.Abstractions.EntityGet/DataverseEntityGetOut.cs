namespace GGroupp.Infra;

public sealed record DataverseEntityGetOut<TEntityJson>
{
    public DataverseEntityGetOut(TEntityJson? value)
        =>
        Value = value;

    public TEntityJson? Value { get; }
}