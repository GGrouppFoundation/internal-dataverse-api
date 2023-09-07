using System;

namespace GarageGroup.Infra;

internal interface IGuidProvider
{
    Guid NewGuid();
}