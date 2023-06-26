namespace GarageGroup.Infra;

public readonly record struct DataverseEntityCreateOut<TOutJson>
{
    public DataverseEntityCreateOut(TOutJson? value)
        =>
        Value = value;

    public TOutJson? Value { get; }
}