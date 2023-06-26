namespace GarageGroup.Infra;

public readonly record struct DataverseEntityUpdateOut<TOutJson>
{
    public DataverseEntityUpdateOut(TOutJson? value)
        =>
        Value = value;

    public TOutJson? Value { get; }
}