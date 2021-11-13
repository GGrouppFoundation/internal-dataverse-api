namespace GGroupp.Infra;

public readonly record struct DataverseEntityCreateOut<TResponseJson>
{
    public DataverseEntityCreateOut(TResponseJson? value)
        =>
        Value = value;

    public TResponseJson? Value { get; }
}