namespace GGroupp.Infra;

public interface IDataverseApiClientConfiguration
{
    string ServiceUrl { get; }

    string ApiType { get; }

    string ApiVersion { get; }

    string ApiSearchType { get; }

    string AuthTenantId { get; }

    string AuthClientId { get; }

    string AuthClientSecret { get; }
}