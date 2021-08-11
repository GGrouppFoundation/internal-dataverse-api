#nullable enable

namespace GGroupp.Infra
{
    public interface IDataverseApiClient : 
        IDataverseEntitySetGetSupplier, IDataverseEntityGetSupplier, 
        IDataverseEntityCreateSupplier, IDataverseEntityDeleteSupplier
    {
    }
}