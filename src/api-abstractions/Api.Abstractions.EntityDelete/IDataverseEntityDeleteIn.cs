﻿using System;

namespace GarageGroup.Infra;

public interface IDataverseEntityDeleteIn : IDataverseTransactableIn<Unit>, IDataverseTransactableIn<object>
{
    string EntityPluralName { get; }

    IDataverseEntityKey EntityKey{ get; }

    Unit IDataverseTransactableIn<Unit>.Entity
        =>
        default;

    object? IDataverseTransactableIn<object>.Entity
        =>
        default;
}