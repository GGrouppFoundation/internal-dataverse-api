using System;

namespace GGroupp.Infra;

public record class DataverseApiClientOption
{
    public DataverseApiClientOption(string serviceUrl)
        =>
        ServiceUrl = serviceUrl.OrEmpty();

    public string ServiceUrl { get; }
}