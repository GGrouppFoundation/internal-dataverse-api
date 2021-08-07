#nullable enable

using System;
using System.Net.Http;

namespace GGroupp.Infra
{
    internal sealed partial class DataverseApiClient : IDataverseApiClient
    {
        private const string LoginMsOnlineServiceBaseUrl = "https://login.microsoftonline.com/";

        public static DataverseApiClient Create(HttpMessageHandler messageHandler, IDataverseApiClientConfiguration clientConfiguration)
            =>
            new(
                messageHandler ?? throw new ArgumentNullException(nameof(messageHandler)),
                clientConfiguration ?? throw new ArgumentNullException(nameof(clientConfiguration)));

        private readonly HttpMessageHandler messageHandler;

        private readonly IDataverseApiClientConfiguration clientConfiguration;

        private DataverseApiClient(HttpMessageHandler messageHandler, IDataverseApiClientConfiguration clientConfiguration)
        {
            this.messageHandler = messageHandler;
            this.clientConfiguration = clientConfiguration;
        }

        private static Failure<int> MapDataverseFailureJson(DataverseFailureJson? failureJson)
            =>
            Pipeline.Pipe(
                failureJson?.Error?.Code)
            .Pipe(
                code => string.IsNullOrEmpty(code) ? default : Convert.ToInt32(code, 16))
            .Pipe(
                code => Failure.Create(code, failureJson?.Error?.Message));
    }
}