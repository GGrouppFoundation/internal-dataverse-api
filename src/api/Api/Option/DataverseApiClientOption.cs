using System;

namespace GarageGroup.Infra;

public record class DataverseApiClientOption
{
    public DataverseApiClientOption(string serviceUrl, TimeSpan? httpTimeOut = null)
    {
        ServiceUrl = serviceUrl.OrEmpty();
        HttpTimeOut = httpTimeOut;
    }

    public string ServiceUrl { get; }
    
    public TimeSpan? HttpTimeOut { get; }
}