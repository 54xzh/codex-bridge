// BridgeRequestAuthorizer：复用 WS/HTTP 的统一鉴权逻辑（默认仅回环，开启远程后使用 BearerToken）。
using System.Net;
using Microsoft.Extensions.Options;

namespace codex_bridge_server.Bridge;

public sealed class BridgeRequestAuthorizer
{
    private readonly IOptions<BridgeSecurityOptions> _securityOptions;

    public BridgeRequestAuthorizer(IOptions<BridgeSecurityOptions> securityOptions)
    {
        _securityOptions = securityOptions;
    }

    public bool IsAuthorized(HttpContext context)
    {
        var options = _securityOptions.Value;
        var remoteIp = context.Connection.RemoteIpAddress;
        var isLoopback = remoteIp is not null && IPAddress.IsLoopback(remoteIp);

        if (!options.RemoteEnabled)
        {
            return isLoopback;
        }

        if (string.IsNullOrWhiteSpace(options.BearerToken))
        {
            return false;
        }

        var authHeader = context.Request.Headers.Authorization.ToString();
        const string prefix = "Bearer ";
        if (!authHeader.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        var token = authHeader[prefix.Length..].Trim();
        return string.Equals(token, options.BearerToken, StringComparison.Ordinal);
    }
}

