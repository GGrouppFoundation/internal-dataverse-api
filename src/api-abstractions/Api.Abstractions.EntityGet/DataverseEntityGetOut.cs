namespace GarageGroup.Infra;

public readonly record struct DataverseEntityGetOut<TEntityJson>
{
    public DataverseEntityGetOut(TEntityJson? value)
        =>
        Value = value;

    public TEntityJson? Value { get; }
}