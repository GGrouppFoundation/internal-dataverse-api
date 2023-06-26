using System;

namespace GarageGroup.Infra;

public interface IDataverseImpersonateSupplier<out T>
{
    T Impersonate(Guid callerId);
}