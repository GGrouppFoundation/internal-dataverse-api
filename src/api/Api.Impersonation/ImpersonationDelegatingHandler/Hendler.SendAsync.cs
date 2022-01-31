using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GGroupp.Infra;

partial class ImpersonationDelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        _ = request ?? throw new ArgumentNullException(nameof(request));
        
        if(request.Headers.Contains(MSCRMCallerID))
        {
            request.Headers.Remove(MSCRMCallerID);
        }

        var callerId = await callerIdProvider.InvokeAsync(cancellationToken).ConfigureAwait(false);

        request.Headers.Add(MSCRMCallerID, callerId.ToString("D", CultureInfo.InvariantCulture));
        
        return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
    }
}