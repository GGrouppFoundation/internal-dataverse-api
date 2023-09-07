using System;

namespace GarageGroup.Infra;

internal sealed class GuidProvider : IGuidProvider
{
    static GuidProvider()
        =>
        Instance = new();

    public static GuidProvider Instance { get; }

    private GuidProvider()
    {
    }

    public Guid NewGuid()
        =>
        Guid.NewGuid();
}
