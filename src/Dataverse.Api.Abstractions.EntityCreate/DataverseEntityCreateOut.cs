#nullable enable

namespace GGroupp.Infra
{
    public sealed record DataverseEntityCreateOut<TResponseJson>
    {
        public DataverseEntityCreateOut(TResponseJson? value)
            =>
            Value = value;

        public TResponseJson? Value { get; }
    }
}