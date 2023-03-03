using System;

namespace GGroupp.Infra;

public record class DataverseApiClientOption
{
    public DataverseApiClientOption(string serviceUrl, TimeSpan? httpTimeOut = null)
        =>
        (ServiceUrl, HttpTimeOut) = (serviceUrl.OrEmpty(), httpTimeOut);

    public string ServiceUrl { get; }
    
    public TimeSpan? HttpTimeOut { get; }
}