using Serilog;
using gateway_api.API.Middleware;
using StackExchange.Redis;
using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, services, configuration) => configuration
    .ReadFrom.Configuration(context.Configuration)
    .ReadFrom.Services(services)
    .Enrich.FromLogContext()
    .WriteTo.Console());

builder.WebHost.UseSentry();

// Add YARP — configuration from appsettings.json
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

// Add CORS — origins from config (appsettings.Production.json overrides for Azure)
var corsOrigins = (builder.Configuration["CorsOrigins"] ?? "http://localhost:4200,https://localhost:4200")
    .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policy =>
        policy
        .WithOrigins(corsOrigins)
        .AllowAnyMethod()
        .AllowAnyHeader());
});

// Add Redis (singleton — one multiplexer for the process lifetime)
var redisConn = builder.Configuration["Redis:ConnectionString"] ?? "localhost:6379";
builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(redisConn));

// Add rate limiting — fixed window, per IP
// Adjust PermitLimit / Window to taste; sliding window or token bucket are also available.
builder.Services.AddRateLimiter(options =>
{
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(ctx =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: ctx.Connection.RemoteIpAddress?.ToString() ?? "anon",
            factory: _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit          = 100,
                Window               = TimeSpan.FromMinutes(1),
                QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                QueueLimit           = 0
            }));

    options.OnRejected = async (context, _) =>
    {
        context.HttpContext.Response.StatusCode = 429;
        await context.HttpContext.Response.WriteAsync("Too many requests.");
    };
});

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Named HttpClient for internal service aggregation (accepts self-signed certs for local dev)
builder.Services.AddHttpClient("internal", client => { })
    .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
    {
        ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
    });

var app = builder.Build();

app.UseCors("CorsPolicy");

app.UseRateLimiter();

// Swagger UI — dynamically built from YARP clusters.
app.UseSwaggerUI(c =>
{
    var clusters = app.Configuration.GetSection("ReverseProxy:Clusters").GetChildren();
    foreach (var cluster in clusters)
    {
        var name = cluster.Key.Replace("-cluster", "");
        c.SwaggerEndpoint($"/swagger/{name}/v1/swagger.json", $"{name} API");
    }
    c.RoutePrefix = "swagger";
});

app.UseMiddleware<GatewayAuthorizationMiddleware>();

// Aggregate available permissions from all downstream services.
app.MapGet("/api/permission", async (IConfiguration config, IHttpClientFactory httpClientFactory) =>
{
    var allPermissions = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
    using var client = httpClientFactory.CreateClient("internal");

    var clusters = config.GetSection("ReverseProxy:Clusters").GetChildren();
    foreach (var cluster in clusters)
    {
        foreach (var dest in cluster.GetSection("Destinations").GetChildren())
        {
            var address = dest["Address"];
            if (string.IsNullOrEmpty(address)) continue;
            try
            {
                var permissions = await client.GetFromJsonAsync<IEnumerable<string>>(
                    $"{address.TrimEnd('/')}/api/Permission");
                if (permissions != null)
                    foreach (var p in permissions) allPermissions.Add(p);
            }
            catch { /* service unavailable, skip */ }
        }
    }

    return Results.Ok(allPermissions.OrderBy(p => p));
});

// Map the reverse proxy endpoints
app.MapReverseProxy();

app.Run();
