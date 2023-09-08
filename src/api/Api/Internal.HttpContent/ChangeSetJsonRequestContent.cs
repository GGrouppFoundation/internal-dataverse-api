using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace GarageGroup.Infra;

internal sealed class ChangeSetJsonRequestContent : HttpContent
{
    private const char Space = ' ';

    private const char Slash = '/';

    private const string ColonSpace = ": ";

    private const string NewLine = "\r\n";

    private const int DefaultBuilderAllocation = 1024;

    private const string DefaultMediaType = "application/http";

    private const string HeaderNameContentTransferEncoding = "Content-Transfer-Encoding";

    private const string RequestMediaType = "application/json; type=entry";

    private const string HeaderNameContentType = "Content-Type";

    private const string HeaderNameContentId = "Content-ID";

    private const string DefaultContentTransferEncoding = "binary";

    private const string HttpVersionToken = "HTTP";

    private const string DefaultHttpVersion = "1.1";

    private readonly byte[] content;

    internal ChangeSetJsonRequestContent(
        DataverseJsonRequest request, int contentId, [AllowNull] string httpVersion = null)
    {
        Headers.Add(HeaderNameContentType, DefaultMediaType);
        Headers.Add(HeaderNameContentTransferEncoding, DefaultContentTransferEncoding);
        Headers.Add(HeaderNameContentId, contentId.ToString());

        var message = BuildContentMessage(request, httpVersion.OrNullIfEmpty() ?? DefaultHttpVersion);
        content = Encoding.UTF8.GetBytes(message);
    }

    private static string BuildContentMessage(DataverseJsonRequest request, string httpVersion)
    {
        var builder = new StringBuilder(DefaultBuilderAllocation);

        builder = builder.Append(request.Verb.ToString("G").ToUpperInvariant()).Append(Space);
        builder = builder.Append(request.Url).Append(Space);
        builder = builder.Append(HttpVersionToken).Append(Slash).Append(httpVersion).Append(NewLine);

        foreach (var header in request.Headers)
        {
            if (string.Equals(header.Name, HeaderNameContentType, StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            builder = builder.Append(header.Name).Append(ColonSpace).Append(header.Value).Append(NewLine);
        }

        if (string.IsNullOrEmpty(request.Content?.Json))
        {
            return builder.Append(NewLine).ToString();
        }

        builder = builder.Append(HeaderNameContentType).Append(ColonSpace).Append(RequestMediaType).Append(NewLine);

        var json = request.Content.Json;
        return builder.Append(NewLine).Append(json).ToString();
    }

    protected override Task SerializeToStreamAsync(Stream stream, TransportContext? context)
    {
        ArgumentNullException.ThrowIfNull(stream);
        return stream.WriteAsync(content, 0, content.Length);
    }

    protected override bool TryComputeLength(out long length)
    {
        length = content.LongLength;
        return true;
    }
}
