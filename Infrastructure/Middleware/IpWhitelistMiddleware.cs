namespace EducationalPlatformApi.Infrastructure.Middleware;

public class IpWhitelistMiddleware
{
    private readonly RequestDelegate _next;
    private readonly string? _whitelist;

    public IpWhitelistMiddleware(RequestDelegate next, IConfiguration config)
    {
        _next = next;
        _whitelist = config["IpWhitelist"];
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var remoteIp = context.Request.Headers["CF-Connecting-IP"].FirstOrDefault() ?? context.Connection.RemoteIpAddress?.ToString();

        if (string.IsNullOrEmpty(_whitelist) || remoteIp != _whitelist)
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            await context.Response.WriteAsync("Access Denied: Your IP address is not whitelisted.");
            return;
        }

        await _next(context);
    }
}