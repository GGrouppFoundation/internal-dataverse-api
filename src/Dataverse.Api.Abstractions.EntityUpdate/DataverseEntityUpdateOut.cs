#nullable enable

namespace GGroupp.Infra
{
    public sealed record DataverseEntityUpdateOut<TResponseJson>
    {
        public DataverseEntityUpdateOut(TResponseJson? value)
            =>
            Value = value;

        public TResponseJson? Value { get; }
    }
}