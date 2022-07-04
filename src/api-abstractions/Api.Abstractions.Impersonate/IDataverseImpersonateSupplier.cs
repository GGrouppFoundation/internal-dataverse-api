using System;

namespace GGroupp.Infra;

public interface IDataverseImpersonateSupplier<out T>
{
    T Impersonate(Guid callerId);
}