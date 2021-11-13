namespace GGroupp.Infra;

public readonly record struct DataverseEntityUpdateOut<TResponseJson>
{
    public DataverseEntityUpdateOut(TResponseJson? value)
        =>
        Value = value;

    public TResponseJson? Value { get; }
}