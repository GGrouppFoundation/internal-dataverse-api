namespace GarageGroup.Infra;

public interface IDataverseApiClient :
    IDataverseImpersonateSupplier<IDataverseApiClient>, IDataverseChangeSetExecuteSupplier,
    IDataverseEntitySetGetSupplier, IDataverseEntityGetSupplier, IDataverseEntityCreateSupplier,
    IDataverseEntityDeleteSupplier, IDataverseEntityUpdateSupplier, IDataverseSearchSupplier, 
    IDataverseWhoAmISupplier, IDataverseEmailSendSupplier, IDataverseFetchXmlSupplier, IPingSupplier;