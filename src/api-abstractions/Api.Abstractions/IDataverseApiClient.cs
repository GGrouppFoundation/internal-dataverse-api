namespace GGroupp.Infra;

public interface IDataverseApiClient :
    IDataverseImpersonateSupplier<IDataverseApiClient>,
    IDataverseEntitySetGetSupplier, IDataverseEntityGetSupplier, IDataverseEntityCreateSupplier,
    IDataverseEntityDeleteSupplier, IDataverseEntityUpdateSupplier, IDataverseSearchSupplier, 
    IDataverseWhoAmISupplier, IDataverseEmailSendSupplier, IDataverseFetchXmlSupplier
{
}