namespace GarageGroup.Infra;

public interface IDataverseTransactableIn<out TIn>
    where TIn : notnull
{
    TIn? Entity { get; }
}